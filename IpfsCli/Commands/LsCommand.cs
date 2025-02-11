﻿using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace Ipfs.Cli {
    [Command(Description = "List links")]
    class LsCommand : CommandBase
    {
        [Argument(0, "ipfs-path", "The path to an IPFS object")]
        [Required]
        public string IpfsPath { get; set; }

        Program Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var node = await Parent.CoreApi.FileSystem.ListFileAsync(IpfsPath);
            return Parent.Output(app, node, (data, writer) =>
            {
                foreach (var link in data.Links)
                {
                    writer.WriteLine($"{link.Id.Encode()} {link.Size} {link.Name}");
                }
            });
        }

    }
}
