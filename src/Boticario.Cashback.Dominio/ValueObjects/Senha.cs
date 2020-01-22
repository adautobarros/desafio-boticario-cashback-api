using Flunt.Notifications;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Boticario.Cashback.Dominio.ValueObjects
{
    public sealed class Senha : Notifiable, IEquatable<Senha>, IEquatable<string>
    {
        public string Encoded { get; }

        public Senha(string senha) : base()
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                AddNotification("senha", "A senha não pode ficar vazia");
                return;
            }

            Encoded = EncodePassword(senha);
        }

        public static implicit operator Senha(string senha) => new Senha(senha);

        public static implicit operator string(Senha value) => value.Encoded;

        private static string EncodePassword(string senha)
        {
            string result;
            var bytes = Encoding.Unicode.GetBytes(senha);

            using (var stream = new MemoryStream())
            {
                stream.WriteByte(0);

                using (var sha256 = new SHA256Managed())
                {
                    var hash = sha256.ComputeHash(bytes);
                    stream.Write(hash, 0, hash.Length);

                    bytes = stream.ToArray();
                    result = Convert.ToBase64String(bytes);
                }

            }
            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Senha;

            return other != null ? Equals(other) : Equals(obj as string);
        }

        public bool Equals(Senha other) => other != null && Encoded == other.Encoded;

        public bool Equals(string other) => Encoded == other;

        public static bool operator ==(Senha a, Senha b)
        {
            if (ReferenceEquals(a, b)) return true;
            if ((a is null) || (b is null)) return false;

            return a.Encoded == b.Encoded;
        }

        public static bool operator !=(Senha a, Senha b) => !(a == b);

        public override int GetHashCode() => Encoded.GetHashCode();

        public override string ToString() => Encoded;
    }
}
