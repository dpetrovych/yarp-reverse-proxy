// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Yarp.Kubernetes.Controller.Certificates;

/// <summary>
/// A interface for configuring server certificates.
/// </summary>
public interface IServerCertificateStore
{
    /// <summary>
    /// Updates default certificate which will be used if domain name didn't match any other.
    /// </summary>
    /// <param name="defaultCertificate">The server certificate, can be set null</param>
    void UpdateDefaultCertificate(X509Certificate2 defaultCertificate);

    /// <summary>
    /// Updates map of certificate by the domain name.
    /// </summary>
    /// <param name="certificates">Map of domain names to certificate.</param>
    void UpdateCertificates(Dictionary<string, X509Certificate2> certificates);
}
