using KetKoin;
using Ketting;
using Ketting_server.Dto;

namespace Ketting_server.Services;

public class BlockChainService
{
    public KetKoinChain KetKoinChain { get; set; }
    public BlockChainService(IConfiguration configuration)
    {
        KetKoinChain = new KetKoinChain();
        KeyPair keyPair = new KeyPair();
        if (Environment.GetEnvironmentVariable("privateKey") != null)
        {
            KetKoinChain.SetKeys(Convert.FromBase64String(Environment.GetEnvironmentVariable("privateKey")));
        }
        else
        {
            KetKoinChain.SetKeys(keyPair.PrivateKey);
        }
    }

    public void AddTransaction(TransactionDto transaction)
    {
        KetKoinChain.AddTransactionToPool(transaction.ToObject());
    }

    public async void GetNewChain(string connection)
    {
        try
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://" + connection + "/blockchain");
            List<BlockDto> blockDtos = await response.Content.ReadFromJsonAsync<List<BlockDto>>();

            List<Block> blocks = new List<Block>();
            foreach (BlockDto block in blockDtos)
            {
                blocks.Add(block.ToObject());
            }

            KetKoinChain.BlockChain = blocks;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}