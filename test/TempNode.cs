﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs.Engine
{
    class TempNode : IpfsEngine
    {
        static int nodeNumber;

        public TempNode()
            : base("xyzzy".ToCharArray())
        {
            Options.Repository.Folder = Path.Combine(Path.GetTempPath(), $"ipfs-{nodeNumber++}");
            Options.KeyChain.DefaultKeyType = "ed25519";
            Config.SetAsync(
                "Addresses.Swarm",
                JToken.FromObject(new string[] { "/ip4/0.0.0.0/tcp/0" })
            ).Wait();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Directory.Exists(Options.Repository.Folder))
            {
                Directory.Delete(Options.Repository.Folder, true);
            }
        }
    }
}
