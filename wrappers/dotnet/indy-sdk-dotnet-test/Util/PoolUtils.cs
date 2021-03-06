﻿using Hyperledger.Indy.PoolApi;
using System.IO;
using System.Threading.Tasks;

namespace Hyperledger.Indy.Test
{
    class PoolUtils
    {
        public static string DEFAULT_POOL_NAME = "default_pool";

        public static string CreateGenesisTxnFile(string filename)
        {
            return CreateGenesisTxnFile(filename, 4);
        }

        public static string CreateGenesisTxnFile(string filename, int nodesCnt)
        {
            var file = EnvironmentUtils.GetTmpPath(filename);
            var testPoolIp = EnvironmentUtils.GetTestPoolIP();

            var defaultTxns = new string[]{
                string.Format("{{\"data\":{{\"alias\":\"Node1\",\"client_ip\":\"{0}\",\"client_port\":9702,\"node_ip\":\"{0}\",\"node_port\":9701,\"services\":[\"VALIDATOR\"]}},\"dest\":\"Gw6pDLhcBcoQesN72qfotTgFa7cbuqZpkX3Xo6pLhPhv\",\"identifier\":\"Th7MpTaRZVRYnPiabds81Y\",\"txnId\":\"fea82e10e894419fe2bea7d96296a6d46f50f93f9eeda954ec461b2ed2950b62\",\"type\":\"0\"}}", testPoolIp),
                string.Format("{{\"data\":{{\"alias\":\"Node2\",\"client_ip\":\"{0}\",\"client_port\":9704,\"node_ip\":\"{0}\",\"node_port\":9703,\"services\":[\"VALIDATOR\"]}},\"dest\":\"8ECVSk179mjsjKRLWiQtssMLgp6EPhWXtaYyStWPSGAb\",\"identifier\":\"EbP4aYNeTHL6q385GuVpRV\",\"txnId\":\"1ac8aece2a18ced660fef8694b61aac3af08ba875ce3026a160acbc3a3af35fc\",\"type\":\"0\"}}", testPoolIp),
                string.Format("{{\"data\":{{\"alias\":\"Node3\",\"client_ip\":\"{0}\",\"client_port\":9706,\"node_ip\":\"{0}\",\"node_port\":9705,\"services\":[\"VALIDATOR\"]}},\"dest\":\"DKVxG2fXXTU8yT5N7hGEbXB3dfdAnYv1JczDUHpmDxya\",\"identifier\":\"4cU41vWW82ArfxJxHkzXPG\",\"txnId\":\"7e9f355dffa78ed24668f0e0e369fd8c224076571c51e2ea8be5f26479edebe4\",\"type\":\"0\"}}", testPoolIp),
                string.Format("{{\"data\":{{\"alias\":\"Node4\",\"client_ip\":\"{0}\",\"client_port\":9708,\"node_ip\":\"{0}\",\"node_port\":9707,\"services\":[\"VALIDATOR\"]}},\"dest\":\"4PS3EDQ3dW1tci1Bp6543CfuuebjFrg36kLAUcskGfaA\",\"identifier\":\"TWwCRQRZ2ZHMJFn9TzLp7W\",\"txnId\":\"aa5e817d7cc626170eca175822029339a444eb0ee8f0bd20d3b0b76e566fb008\",\"type\":\"0\"}}", testPoolIp)
             };

            Directory.CreateDirectory(Path.GetDirectoryName(file));
            var stream = new StreamWriter(file);

            for (int i = 0; i < nodesCnt; i++)
            {
                stream.WriteLine(defaultTxns[i]);
            }

            stream.Close();

            return file;
        }

        public static string CreatePoolLedgerConfig()
        {
            return CreatePoolLedgerConfig(4);
        }

        public static string CreatePoolLedgerConfig(int nodesCnt)
        {
            var poolName = "test";

            CreatePoolLedgerConfig(poolName, nodesCnt);
            return poolName;
        }

        public static void CreatePoolLedgerConfig(string poolName, int nodesCnt)
        {
            var genesisTxnFile = CreateGenesisTxnFile("temp.txn", nodesCnt);
            var path = Path.GetFullPath(genesisTxnFile).Replace('\\', '/');
            var createPoolLedgerConfig = string.Format("{{\"genesis_txn\":\"{0}\"}}", path);

            Pool.CreatePoolLedgerConfigAsync(poolName, createPoolLedgerConfig);
        }

        public static async Task<Pool> CreateAndOpenPoolLedgerAsync()
        {
            var poolName = PoolUtils.CreatePoolLedgerConfig();
            var openPoolLedgerConfig = "{\"refresh_on_open\":true}";

            return await Pool.OpenPoolLedgerAsync(poolName, openPoolLedgerConfig);
        }        
    }
}

