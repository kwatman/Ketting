using KetKoin;
using Ketting;

namespace Ketting_server.Dto;

public class BlockDto
{
    public string PrevHash { get; set; }
    public int Version { get; set; }
    public List<TransactionDto> Data { get; set; }
    public DateTime Timestamp { get; set; }
    public string PublicKey { get; set; }
    public string Hash { get; set; }
    public string Signature { get; set; }

    public BlockDto()
    {
        
    }

    public BlockDto(Block block)
    {
        FromObject(block);
    }

    public void FromObject(Block block)
    {
        Data = new List<TransactionDto>();
        PrevHash = block.PrevHash;
        Version = block.Version;
        foreach (Transaction transaction in block.Data)
        {
            TransactionDto transactionDto = new TransactionDto();
            transactionDto.FromObject(transaction);
            Data.Add(transactionDto);
        }

        Timestamp = block.Timestamp;
        PublicKey = block.PublicKey;
        Hash = block.Hash;
        Signature = block.Signature;
    }

    public Block ToObject()
    {
        Block block = new Block();
        block.PrevHash = PrevHash;
        block.Version = Version;
        foreach (TransactionDto transaction in Data)
        {
            block.Data.Add(transaction.ToObject());
        }
        block.Timestamp = Timestamp;
        block.PublicKey = PublicKey;
        block.Hash = Hash;
        block.Signature = Signature;
        return block;
    }
}