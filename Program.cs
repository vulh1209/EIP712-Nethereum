using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nethereum.Signer;

public class TestSignNEthereum
{
	static void Main(string[] args)
	{
		var key = new EthECKey("8f455772a29ba1314c585d45910896143836c0beb116dfc30b909541da2bc8ad");
		Console.WriteLine("Signing address: " + key.GetPublicAddress());

        var transactionRequest = new TransactionRequest
        {
            sender = "477322774629",
            details = new List<TransactionDetail>
            {
                new TransactionDetail
				{
					itemId = "ddcc4d2f-a2e3-4018-83a7-6e104f2d3dd0",
                    transactionType = TransactionType.MintRam,
                    assetId = "TOKEN:RAM:RAM:0",
                    tokenId = "0",
                    value = "123",
                    sender = "0x83629905189464CC16F5E7c12D54dD5e87459B33",
                    receiver = "0x83629905189464CC16F5E7c12D54dD5e87459B33",
                },
            },
            execute = true,
			requestId = "123456789",
			signatureExpire = 1778184688189,
			chainId = 137,	
        };
		
		var signer = new EthereumMessageSigner();
		var msg = JsonConvert.SerializeObject(transactionRequest);
        Console.WriteLine("message: " + msg);
		var signature = signer.EncodeUTF8AndSign(msg,key);
        Console.WriteLine("Signature: " + signature);

    }

	public enum TransactionType {
		Unspecified = 0,
		Transfer = 1,
		Mint = 2,
		Burn = 3,
		Deposit = 4,
		Withdraw = 5,
		Lock = 6,
		Release = 7,
		BurnAllNonWithdrawable = 8,
		TransferAll = 9,
		TransferAllWithdrawable = 10,
	}

	public class TransactionRequest
	{
		public string sender { get; set; }
        public List<TransactionDetail> details { get; set; }
		public bool execute { get; set; }
		public string requestId { get; set; }
		public int chainId { get; set; }
		public long signatureExpire { get; set; }

	}

	public class TransactionDetail
	{
		public string itemId { get; set; }
		public TransactionType transactionType { get; set; }
		public string assetId { get; set; }
		public string tokenId { get; set; }
		public string value { get; set; }
		public string sender { get; set; }
		public string receiver { get; set; }
	}

}