using KetKoin;
using Ketting;
using Ketting_server.Dto;

namespace Ketting_server.Services;

public class BroadcastService
{
    public async void BroadCastTransaction(Transaction transaction, List<string> connections)
    {
        TransactionDto transactionDto = new TransactionDto();
        transactionDto.FromObject(transaction);
        foreach (string ip in connections)
        {
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsJsonAsync("http://" + ip + "/transaction", transactionDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public async void BroadCastBlockMint(Block block, List<string> connections)
    {
        BlockDto blockDto = new BlockDto();
        blockDto.FromObject(block);
        foreach (string ip in connections)
        {
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsJsonAsync("http://" + ip + "/blockchain/block", blockDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}