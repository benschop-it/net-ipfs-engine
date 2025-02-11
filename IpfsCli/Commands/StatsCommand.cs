﻿using McMaster.Extensions.CommandLineUtils;

namespace Ipfs.Cli {
    [Command(Description = "Query IPFS statistics")]
    [Subcommand("bw", typeof(StatsBandwidthCommand))]
    [Subcommand("repo", typeof(StatsRepoCommand))]
    [Subcommand("bitswap", typeof(StatsBitswapCommand))]
    class StatsCommand : CommandBase
    {
        public Program Parent { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }
    }

    [Command(Description = "IPFS bandwidth information")]
    class StatsBandwidthCommand : CommandBase
    {
        StatsCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.Stats.BandwidthAsync();
            return Program.Output(app, stats, null);
        }
    }

    [Command(Description = "Repository information")]
    class StatsRepoCommand : CommandBase
    {
        StatsCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.Stats.RepositoryAsync();
            return Program.Output(app, stats, null);
        }
    }

    [Command(Description = "Bitswap information")]
    class StatsBitswapCommand : CommandBase
    {
        StatsCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.Stats.BitswapAsync();
            return Program.Output(app, stats, null);
        }
    }

}
