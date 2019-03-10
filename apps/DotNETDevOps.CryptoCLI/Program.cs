using IdentityModel;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace DotNETDevOps.CryptoCLI
{


    [Command(Name = "crypto", Description = "crypto helper")]
    [HelpOption("-?")]
    class Program
    {
        static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

        [Required]
        [Option("-t|--thumbprint <THUMBPRINT>", "The thumbprint to use for encrypting", CommandOptionType.SingleValue)]
        public string Thumbprint { get; set; }


        [Argument(0, Description = "The value to encrypt")]
        private string Value { get; }

        private Task<int> OnExecuteAsync(CommandLineApplication app)
        {

            var encoded = Encoding.Unicode.GetBytes(Value);
             
            var cert = X509.LocalMachine.My.Thumbprint.Find(Thumbprint, validOnly: false).FirstOrDefault();

            var content = new ContentInfo(encoded);
            var env = new EnvelopedCms(content);
            env.Encrypt(new CmsRecipient(cert));


            Console.WriteLine(Convert.ToBase64String(env.Encode()));

            return Task.FromResult(0);

        }
    }
}
