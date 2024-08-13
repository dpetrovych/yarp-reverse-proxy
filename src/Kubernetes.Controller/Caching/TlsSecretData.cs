using System;
using System.Security.Cryptography.X509Certificates;
using k8s.Models;
using Yarp.Kubernetes.Controller.Certificates;

namespace Yarp.Kubernetes.Controller.Caching;

/// <summary>
/// Holds data needed from a <see cref="V1Secret"/> resource of TLS type.
/// </summary>
public struct TlsSecretData
{
    public TlsSecretData(V1Secret secret)
    {
        if (secret is null)
        {
            throw new ArgumentNullException(nameof(secret));
        }

        if (!IsTlsSecret(secret))
        {
            throw new ArgumentException("Secret is not of type kubernetes.io/tls", nameof(secret));
        }

        Name = secret.Name();
        Certificate = CertificateHelper.ConvertCertificate(secret);
    }

    public string Name { get; }

    public X509Certificate2 Certificate { get; }

    public static bool IsTlsSecret(V1Secret secret)
    {
        return secret.Type == "kubernetes.io/tls";
    }
}
