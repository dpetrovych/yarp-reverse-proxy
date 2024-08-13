// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.AspNetCore.Connections;

namespace Yarp.Kubernetes.Controller.Certificates;

internal class ServerCertificateSelector : IServerCertificateSelector, IServerCertificateStore
{
    private X509Certificate2 _defaultCertificate;

    private volatile Dictionary<string, X509Certificate2> _certificates = new Dictionary<string, X509Certificate2>();

    public X509Certificate2 GetCertificate(ConnectionContext connectionContext, string domainName)
    {
        if(_certificates.TryGetValue(domainName, out var certificate))
        {
            return certificate;
        }

        return _defaultCertificate;
    }

    public void UpdateCertificates(Dictionary<string, X509Certificate2> certificates)
    {
        if (certificates is null)
        {
            throw new ArgumentNullException(nameof(certificates));
        }

        // recreate the dictionary to ensure that:
        // 1. comparer is case insensitive
        // 2. the dictionary is not modified while beeing read (thread safety)
        var newCertificates = new Dictionary<string, X509Certificate2>(certificates, StringComparer.OrdinalIgnoreCase);

        Interlocked.Exchange(ref _certificates, newCertificates);
    }

    public void UpdateDefaultCertificate(X509Certificate2 defaultCertificate)
    {
        _defaultCertificate = defaultCertificate;
    }

    void IServerCertificateSelector.AddCertificate(NamespacedName certificateName, X509Certificate2 certificate)
    {
        UpdateDefaultCertificate(certificate);
    }

    void IServerCertificateSelector.RemoveCertificate(NamespacedName certificateName)
    {
        _defaultCertificate = null;
    }
}
