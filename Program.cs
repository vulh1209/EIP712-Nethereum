using System;
using System.Text;
using System.Collections.Generic;
using Nethereum.Signer.EIP712;
using Nethereum.Util;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer;
using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;

public class TypedDataSigningUsingEIP712_V4
{
	static void Main(string[] args)
	{
		var signer = new Eip712TypedDataSigner();

		var key = new EthECKey("8f455772a29ba1314c585d45910896143836c0beb116dfc30b909541da2bc8ad");
		Console.WriteLine("Signing address: " + key.GetPublicAddress());

		var typedData = GetTransactionTypedDefinition();

        //The data we are going to sign (Primary type) transaction request
        var transactionRequest = new TransactionRequest
        {
            Sender = "0x83629905189464CC16F5E7c12D54dD5e87459B33",
            Details = new List<TransactionDetail>
            {
                new TransactionDetail
				{
                    Sender = "0x83629905189464CC16F5E7c12D54dD5e87459B33",
                    Receiver = "0x83629905189464CC16F5E7c12D54dD5e87459B33",
                    TransactionType = "Transfer",
                    AssetId = "0",
                    TokenId = "1",
                    Value = 1,
                },
            },
            Execute = true,
        };
		Console.WriteLine("typedData: " + typedData.PrimaryType);
		var signature = signer.SignTypedDataV4(transactionRequest, typedData, key);
        Console.WriteLine("Signature: " + signature);
        var addressRecovered = signer.RecoverFromSignatureV4(transactionRequest, typedData, signature);
        var address = key.GetPublicAddress();
        Console.WriteLine("Recovered address from signature:" + addressRecovered);
    }

	public static TypedData<Domain> GetTransactionTypedDefinition()
	{
		return new TypedData<Domain>
		{
			Domain = new Domain
			{
				Name = "balance-keeper",
				Version = "1",
				ChainId = 0,
				VerifyingContract = "0x83629905189464CC16F5E7c12D54dD5e87459B33"
			},
			Types = MemberDescriptionFactory.GetTypesMemberDescription(typeof(Domain), typeof(TransactionRequest), typeof(TransactionDetail)),
			PrimaryType = nameof(TransactionRequest),
		};
	}

	[Struct("TransactionRequest")]
	public class TransactionRequest
	{
		[Parameter("address", "sender", 1)]
		public string Sender { get; set; }

		[Parameter("tuple[]", "details", 2, "TransactionDetail[]")]
        public List<TransactionDetail> Details { get; set; }

        [Parameter("bool", "execute", 3)]
		public bool Execute { get; set; }
	}

	[Struct("TransactionDetail")]
	public class TransactionDetail
	{
		[Parameter("address", "sender", 1)]
		public string Sender { get; set; }

		[Parameter("address", "receiver", 2)]
		public string Receiver { get; set; }

		[Parameter("string", "transactionType", 3)]
		public string TransactionType { get; set; }

		[Parameter("string", "assetId", 4)]
		public string AssetId { get; set; }

		[Parameter("string", "tokenId", 5)]
		public string TokenId { get; set; }

		[Parameter("uint256", "value", 6)]
		public BigInteger Value { get; set; }
	}

}