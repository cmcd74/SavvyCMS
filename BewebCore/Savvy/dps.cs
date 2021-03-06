using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Beweb;

namespace Savvy
{
	/// <summary>
	/// DPS functionality
	/// </summary>
	public class DPS
	{
		//TODO: add better descriptions
		// grab them from http://www.paymentexpress.com/technical_resources/ecommerce_hosted/pxpay.html
		//TODO: throw exceptions when required values are empty or when values are wrong format or out of range

		#region Common Properties

		/// <summary>
		/// optional string: Needs to be generated to add a card for recurring billing and sent again when rebilling transactions.
		/// </summary>
		public string BillingId
		{
			get { return _BillingId; }
			set { _BillingId = value; }
		}
		protected string _BillingId;
		
		/// <summary>
		/// string (NZD): currency to settle in
		/// </summary>
		public string CurrencyInput
		{
			get { return _CurrencyInput; }
			set { _CurrencyInput = value; }
		}
		protected string _CurrencyInput;
		
		/// <summary>
		/// optional string: The BillingId generated by DPS when adding a card for recurring billing. Needed for rebilling transactions when you do not use your own BillingId.
		/// </summary>
		public string DpsBillingId
		{
			get { return _DpsBillingId; }
			set { _DpsBillingId = value; }
		}
		protected string _DpsBillingId;
		
		/// <summary>
		/// optional string (0000000404e81fae): DPS transaction reference. Sent back to DPS for refund and complete transactions.
		/// </summary>
		public string DpsTxnRef
		{
			get { return _DpsTxnRef; }
			set { _DpsTxnRef = value; }
		}
		protected string _DpsTxnRef;
		
		/// <summary>
		/// optional string
		/// </summary>
		public string EmailAddress
		{
			get { return _EmailAddress; }
			set { _EmailAddress = value; }
		}
		protected string _EmailAddress;
		
		/// <summary>
		/// Reference field to appear on transaction reports
		/// </summary>
		public string MerchantReference
		{
			get { return _MerchantReference; }
			set { _MerchantReference = value; }
		}
		protected string _MerchantReference;
		
		/// <summary>
		/// string: Your account's 64 character key
		/// </summary>
		public string PxPayKey
		{
			get { return _PxPayKey; }
			set { _PxPayKey = value; }
		}
		protected string _PxPayKey;
		
		/// <summary>
		/// string: Your account's UserId
		/// </summary>
		public string PxPayUserId
		{
			get { return _PxPayUserId; }
			set { _PxPayUserId = value; }
		}
		protected string _PxPayUserId;
		
		/// <summary>
		/// optional string: user data passed around
		/// </summary>
		public string TxnData1
		{
			get { return _TxnData1; }
			set { _TxnData1 = value; }
		}
		protected string _TxnData1;
		
		/// <summary>
		/// optional string: user data passed around
		/// </summary>
		public string TxnData2
		{
			get { return _TxnData2; }
			set { _TxnData2 = value; }
		}
		protected string _TxnData2;
		
		/// <summary>
		/// optional string: user data passed around
		/// </summary>
		public string TxnData3
		{
			get { return _TxnData3; }
			set { _TxnData3 = value; }
		}
		protected string _TxnData3;
		
		/// <summary>
		/// optional string (eg P7275C52E1182518): if you don't create one DPS makes one for you. If you do, it should be unique. It is passed around.
		/// </summary>
		public string TxnId
		{
			get { return _TxnId; }
			set { _TxnId = value; }
		}
		protected string _TxnId;
		
		/// <summary>
		/// string 'Purchase', 'Auth', 'Complete', 'Refund', 'Validate'
		/// </summary>
		public string TxnType
		{
			get { return _TxnType; }
			set { _TxnType = value; }
		}
		protected string _TxnType = "Purchase";

		#endregion

		public DPS()
		{
			// constructor values here
			_PxPayUserId = Util.GetSetting("DPS_PxPayUserId", "throw");
			_PxPayKey = Util.GetSetting("DPS_PxPayKey", "throw");
			_CurrencyInput = Util.GetSetting("DPS_CurrencyInput", "throw");
		}

		public class GenerateRequest : DPS
		{
			#region Properties
			
			protected decimal _AmountInput;
			public decimal AmountInput
			{
				get { return _AmountInput; }
				set { _AmountInput = value; }
			}
			protected string _EnableAddBillCard;
			public string EnableAddBillCard
			{
				get { return _EnableAddBillCard; }
				set { _EnableAddBillCard = value; }
			}
			protected string _UrlFail;
			public string UrlFail
			{
				get { return _UrlFail; }
				set { _UrlFail = value; }
			}
			protected string _UrlSuccess;
			public string UrlSuccess
			{
				get { return _UrlSuccess; }
				set { _UrlSuccess = value; }
			}			
			
			#endregion

			public string GetRedirect()
			{
				string returnVal = "";

				StringBuilder xmlString = new StringBuilder();
				xmlString.Append("<GenerateRequest>");
				AddXmlElement("PxPayUserId", _PxPayUserId, xmlString);
				AddXmlElement("PxPayKey", _PxPayKey, xmlString);
				AddXmlElement("AmountInput", String.Format("{0:0.00}", _AmountInput), xmlString);
				AddXmlElement("BillingId", _BillingId, xmlString);
				AddXmlElement("CurrencyInput", _CurrencyInput, xmlString);
				AddXmlElement("DpsBillingId", _DpsBillingId, xmlString);
				AddXmlElement("DpsTxnRef", _DpsTxnRef, xmlString);
				AddXmlElement("EmailAddress", _EmailAddress, xmlString);
				AddXmlElement("EnableAddBillCard", _EnableAddBillCard, xmlString);
				AddXmlElement("MerchantReference", _MerchantReference, xmlString);
				AddXmlElement("TxnData1", _TxnData1, xmlString);
				AddXmlElement("TxnData2", _TxnData2, xmlString);
				AddXmlElement("TxnData3", _TxnData3, xmlString);
				AddXmlElement("TxnType", _TxnType, xmlString);
				AddXmlElement("TxnId", _TxnId, xmlString);
				AddXmlElement("UrlFail", _UrlFail, xmlString);
				AddXmlElement("UrlSuccess", _UrlSuccess, xmlString);
				xmlString.Append("</GenerateRequest>\n");

		//string xml = @"<GenerateRequest><PxPayUserId>BlackBarnVineyards_Dev</PxPayUserId><PxPayKey>d28f70ccb81b101ca2e6a3b9469492cbf60b6bdf1ff9951ecc4bf039e65297f4</PxPayKey><TxnType>Purchase</TxnType><CurrencyInput>NZD</CurrencyInput><AmountInput>1.00</AmountInput><MerchantReference>1029440456782VIM</MerchantReference><EmailAddress>jeremy@beweb.co.nz</EmailAddress><UrlSuccess>http://drunk/BlackBarn/eventorder_conf_BlackBarnVineyards_Dev.asp</UrlSuccess><UrlFail>http://drunk/BlackBarn/eventorder_conf_BlackBarnVineyards_Dev.asp</UrlFail></GenerateRequest>";
		//xmlString = new StringBuilder(xml);

				//Web.Write(xmlString.ToString().HtmlEncode() +"<br>");

				//Web.Write("Http Get: " + Util.HttpGet("https://sec.paymentexpress.com/pxpay/pxaccess.aspx")+"<br>");
				//var webclient = new WebClient();
				//returnVal = webclient.UploadString("https://sec.paymentexpress.com/pxpay/pxaccess.aspx", xml);
				//Web.Write("try this:" +returnVal+"<br>");
				
				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create("https://sec.paymentexpress.com/pxpay/pxaccess.aspx");
				webReq.Method = "POST";

				byte[] reqBytes = null;
				reqBytes = Encoding.UTF8.GetBytes(xmlString.ToString());
				webReq.ContentType = "application/x-www-form-urlencoded";
				webReq.ContentLength = reqBytes.Length;
				Stream requestStream = webReq.GetRequestStream();
				requestStream.Write(reqBytes, 0, reqBytes.Length);
				requestStream.Close();

				HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse();
				
				StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.ASCII);

				string pxRequest;
				pxRequest = streamReader.ReadToEnd();
				streamReader.Close();

				XmlDocument xmldoc = new XmlDocument();
				xmldoc.LoadXml(pxRequest);  // MN 20110620 - removed ToLower() as this is in fact case sensitive (now at least)
				string isValid = xmldoc.DocumentElement.Attributes["valid"].Value;
				if (isValid == "1") {
					returnVal = xmldoc.SelectSingleNode("//Request/URI").InnerText;
				} else {
					throw new ProgrammingErrorException("DPS Error Generating Payment Express Request: " + xmldoc.InnerText);
					//returnVal = "ERROR: Invalid request";
				}
				//Web.WriteLine("" + xmldoc.InnerXml.HtmlEncode());
				//Web.End();
				return returnVal;
			}
		}

		public class Response : DPS
		{
			#region Properties
			
			/// <summary>
			/// string: raw XML returned from DPS on response - output this for debugging
			/// </summary>
			public string XML
			{
				get { return _XML; }
				set { _XML = value; }
			}
			protected string _XML;
			
			protected string _Valid;
			
			/// <summary>
			/// currency (00.00): amount charged to card
			/// </summary>
			public string AmountSettlement
			{
				get { return _AmountSettlement; }
				set { _AmountSettlement = value; }
			}
			protected string _AmountSettlement;
			/// <summary>
			/// int (060450): auth code returned from card provider?
			/// </summary>
			public string AuthCode
			{
				get { return _AuthCode; }
				set { _AuthCode = value; }
			}
			protected string _AuthCode;
			/// <summary>
			/// string: cardholder's name
			/// </summary>
			public string CardHolderName
			{
				get { return _CardHolderName; }
				set { _CardHolderName = value; }
			}
			protected string _CardHolderName;
			/// <summary>
			/// string (Visa, MasterCard, Bankcard etc): the card used
			/// </summary>
			public string CardName
			{
				get { return _CardName; }
				set { _CardName = value; }
			}
			protected string _CardName;
			/// <summary>
			/// string (411111........11): incomplete number of the card used 
			/// </summary>
			public string CardNumber
			{
				get { return _CardNumber; }
				set { _CardNumber = value; }
			}
			protected string _CardNumber;
			/// <summary>
			/// string (219.89.80.239): IP address of the user paying
			/// </summary>
			public string ClientInfo
			{
				get { return _ClientInfo; }
				set { _ClientInfo = value; }
			}
			protected string _ClientInfo;
			/// <summary>
			/// string (NZD): can be blank, currency settled in
			/// </summary>
			public string CurrencySettlement
			{
				get { return _CurrencySettlement; }
				set { _CurrencySettlement = value; }
			}
			protected string _CurrencySettlement;
			/// <summary>
			/// int (0609): card expiry date
			/// </summary>
			public string DateExpiry
			{
				get { return _DateExpiry; }
				set { _DateExpiry = value; }
			}
			protected string _DateExpiry;
			/// <summary>
			/// bit (1|0): where 1 = success. Similar to ResponseText
			/// </summary>
			public string Success
			{
				get { return _Success; }
				set { _Success = value; }
			}
			protected string _Success;
			/// <summary>
			/// string (APPROVED|DECLINED): similar to Success
			/// </summary>
			public string ResponseText
			{
				get { return _ResponseText; }
				set { _ResponseText = value; }
			}
			protected string _ResponseText;
			/// <summary>
			/// string (7da9935b): unknown
			/// </summary>
			public string TxnMac
			{
				get { return _TxnMac; }
				set { _TxnMac = value; }
			}
			protected string _TxnMac;
			
			#endregion

			// constructor
			public Response()
			{
				string EncryptedResponse = HttpContext.Current.Request["result"];
				try
				{
					StringBuilder xmlString = new StringBuilder();
					xmlString.Append("<ProcessResponse>");
					AddXmlElement("PxPayUserId", _PxPayUserId, xmlString);
					AddXmlElement("PxPayKey", _PxPayKey, xmlString);
					AddXmlElement("Response", EncryptedResponse, xmlString);
					xmlString.Append("</ProcessResponse>\n");

					HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create("https://sec.paymentexpress.com/pxpay/pxaccess.aspx");
					webReq.Method = "POST";

					byte[] reqBytes = null;
					reqBytes = Encoding.UTF8.GetBytes(xmlString.ToString());
					webReq.ContentType = "application/x-www-form-urlencoded";
					webReq.ContentLength = reqBytes.Length;
					Stream requestStream = webReq.GetRequestStream();
					requestStream.Write(reqBytes, 0, reqBytes.Length);
					requestStream.Close();

					HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse();
					StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.ASCII);

					_XML = streamReader.ReadToEnd();
					streamReader.Close();

				}
				catch (Exception)
				{
					throw new Exception("ERROR: processing DPS response");
				}
				
				
				// now save the values from that response into the right properties
				if (!String.IsNullOrEmpty(_XML))
				{					
					XmlDocument xmldoc = new XmlDocument();
					xmldoc.LoadXml(_XML.ToLower());
					_Valid = xmldoc.DocumentElement.Attributes["valid"].Value;
					if (_Valid == "1")
					{
						_AmountSettlement = xmldoc.SelectSingleNode("//response/amountsettlement").InnerText;
						_AuthCode = xmldoc.SelectSingleNode("//response/authcode").InnerText;
						_BillingId = xmldoc.SelectSingleNode("//response/billingid").InnerText;
						_CardHolderName = xmldoc.SelectSingleNode("//response/cardholdername").InnerText;
						_CardName = xmldoc.SelectSingleNode("//response/cardname").InnerText;
						_CardNumber = xmldoc.SelectSingleNode("//response/cardnumber").InnerText;
						_ClientInfo = xmldoc.SelectSingleNode("//response/clientinfo").InnerText;
						_CurrencyInput = xmldoc.SelectSingleNode("//response/currencyinput").InnerText;
						_CurrencySettlement = xmldoc.SelectSingleNode("//response/currencysettlement").InnerText;
						_DateExpiry = xmldoc.SelectSingleNode("//response/dateexpiry").InnerText;
						_DpsBillingId = xmldoc.SelectSingleNode("//response/dpsbillingid").InnerText;
						_DpsTxnRef = xmldoc.SelectSingleNode("//response/dpstxnref").InnerText;
						_EmailAddress = xmldoc.SelectSingleNode("//response/emailaddress").InnerText;
						_MerchantReference = xmldoc.SelectSingleNode("//response/merchantreference").InnerText;
						_ResponseText = xmldoc.SelectSingleNode("//response/responsetext").InnerText;
						_Success = xmldoc.SelectSingleNode("//response/success").InnerText;
						_TxnData1 = xmldoc.SelectSingleNode("//response/txndata1").InnerText;
						_TxnData2 = xmldoc.SelectSingleNode("//response/txndata2").InnerText;
						_TxnData3 = xmldoc.SelectSingleNode("//response/txndata3").InnerText;
						_TxnId = xmldoc.SelectSingleNode("//response/txnid").InnerText;
						_TxnType = xmldoc.SelectSingleNode("//response/txntype").InnerText;
						_TxnMac = xmldoc.SelectSingleNode("//response/txnmac").InnerText;
					}
				}
			}
		}
		
		/// <summary>
		/// This is required for rebilling
		/// </summary>
		public class PxPostTransaction : DPS
		{
			#region Properties
			
			protected decimal _Amount;
			private string _PxPostUsername;
			private string _PxPostPassword;

			public decimal Amount
			{
				get { return _Amount; }
				set { _Amount = value; }
			}		
			#endregion

			protected string _EnableAddBillCard;
			public string EnableAddBillCard
			{
				get { return _EnableAddBillCard; }
				set { _EnableAddBillCard = value; }
			}

			public PxPostTransaction() {
				_PxPostUsername = Util.GetSetting("DPS_PxPostUsername", "throw");
				_PxPostPassword = Util.GetSetting("DPS_PxPostPassword", "throw");
			}

			public Response ProcessPayment()
			{
				Response returnVal;

				StringBuilder xmlString = new StringBuilder();
				xmlString.Append("<Txn>");
				AddXmlElement("PostUsername", _PxPostUsername, xmlString);
				AddXmlElement("PostPassword", _PxPostPassword, xmlString);
				AddXmlElement("Amount", String.Format("{0:0.00}", _Amount), xmlString);
				Debug.Assert(_BillingId != null, "_BillingId != null");
				AddXmlElement("BillingId", _BillingId, xmlString);
				AddXmlElement("InputCurrency", _CurrencyInput, xmlString);
				AddXmlElement("DpsBillingId", _DpsBillingId, xmlString);
				AddXmlElement("DpsTxnRef", _DpsTxnRef, xmlString);
				AddXmlElement("MerchantReference", _MerchantReference, xmlString);
				AddXmlElement("EnableAddBillCard", _EnableAddBillCard, xmlString);
				AddXmlElement("TxnData1", _TxnData1, xmlString);
				AddXmlElement("TxnData2", _TxnData2, xmlString);
				AddXmlElement("TxnData3", _TxnData3, xmlString);
				AddXmlElement("TxnType", _TxnType, xmlString);
				AddXmlElement("TxnId", _TxnId, xmlString);
				xmlString.Append("</Txn>\n");

				HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create("https://sec.paymentexpress.com/pxpost.aspx");
				webReq.Method = "POST";

				byte[] reqBytes = null;
				reqBytes = Encoding.UTF8.GetBytes(xmlString.ToString());
				webReq.ContentType = "application/x-www-form-urlencoded";
				webReq.ContentLength = reqBytes.Length;
				Stream requestStream = webReq.GetRequestStream();
				requestStream.Write(reqBytes, 0, reqBytes.Length);
				requestStream.Close();

				HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse();
				StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.ASCII);

				string pxRequest;
				pxRequest = streamReader.ReadToEnd();
				streamReader.Close();

				try {
					returnVal = new PxPostResponse(pxRequest);
				} catch {
					throw new ProgrammingErrorException("DPS Error Generating Payment Express Request: " + pxRequest);
				}

				return returnVal;
			}
		}

		public class PxPostResponse : Response {
			#region Properties

			/// <summary>
			/// string: raw XML returned from DPS on response - output this for debugging
			/// </summary>
			public new  string XML {
				get { return _XML; }
				set { _XML = value; }
			}
			protected new string _XML;

			protected new string _Valid;

			/// <summary>
			/// currency (00.00): amount charged to card
			/// </summary>
			public new string AmountSettlement {
				get { return _AmountSettlement; }
				set { _AmountSettlement = value; }
			}
			protected new string _AmountSettlement;
			/// <summary>
			/// int (060450): auth code returned from card provider?
			/// </summary>
			public new string AuthCode {
				get { return _AuthCode; }
				set { _AuthCode = value; }
			}
			protected new string _AuthCode;
			/// <summary>
			/// string: cardholder's name
			/// </summary>
			public new string CardHolderName {
				get { return _CardHolderName; }
				set { _CardHolderName = value; }
			}
			protected new string _CardHolderName;
			/// <summary>
			/// string (Visa, MasterCard, Bankcard etc): the card used
			/// </summary>
			public new string CardName {
				get { return _CardName; }
				set { _CardName = value; }
			}
			protected new string _CardName;
			/// <summary>
			/// string (411111........11): incomplete number of the card used 
			/// </summary>
			public new string CardNumber {
				get { return _CardNumber; }
				set { _CardNumber = value; }
			}
			protected new string _CardNumber;
			/// <summary>
			/// string (219.89.80.239): IP address of the user paying
			/// </summary>
			public new string ClientInfo {
				get { return _ClientInfo; }
				set { _ClientInfo = value; }
			}
			protected new string _ClientInfo;
			/// <summary>
			/// string (NZD): can be blank, currency settled in
			/// </summary>
			public new string CurrencySettlement {
				get { return _CurrencySettlement; }
				set { _CurrencySettlement = value; }
			}
			protected new string _CurrencySettlement;
			/// <summary>
			/// int (0609): card expiry date
			/// </summary>
			public new string DateExpiry {
				get { return _DateExpiry; }
				set { _DateExpiry = value; }
			}
			protected new string _DateExpiry;
			/// <summary>
			/// bit (1|0): where 1 = success. Similar to ResponseText
			/// </summary>
			public new string Success {
				get { return _Success; }
				set { _Success = value; }
			}
			protected new string _Success;
			/// <summary>
			/// string (APPROVED|DECLINED): similar to Success
			/// </summary>
			public new string ResponseText {
				get { return _ResponseText; }
				set { _ResponseText = value; }
			}
			protected new string _ResponseText;
			/// <summary>
			/// string (7da9935b): unknown
			/// </summary>
			public new string TxnMac {
				get { return _TxnMac; }
				set { _TxnMac = value; }
			}
			protected new string _TxnMac;
			protected string _HelpText;

			#endregion

			// constructor
			public PxPostResponse(string xml) {
				_XML = xml;

				// now save the values from that response into the right properties
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.LoadXml(_XML);
				_AmountSettlement = xmldoc.SelectSingleNode("//Amount").InnerText;
				_AuthCode = xmldoc.SelectSingleNode("//AuthCode").InnerText;
				_BillingId = xmldoc.SelectSingleNode("//BillingId").InnerText;
				_CardHolderName = xmldoc.SelectSingleNode("//CardHolderName").InnerText;
				_CardName = xmldoc.SelectSingleNode("//CardName").InnerText;
				_CardNumber = xmldoc.SelectSingleNode("//CardNumber").InnerText;
				//_ClientInfo = xmldoc.SelectSingleNode("//clientinfo").InnerText;
				_CurrencyInput = xmldoc.SelectSingleNode("//CurrencyName").InnerText;
				_CurrencySettlement = xmldoc.SelectSingleNode("//InputCurrencyName").InnerText;
				_DateExpiry = xmldoc.SelectSingleNode("//CurrencyName").InnerText;
				_DpsBillingId = xmldoc.SelectSingleNode("//DpsBillingId").InnerText;
				_DpsTxnRef = xmldoc.SelectSingleNode("//DpsTxnRef").InnerText;
				//_EmailAddress = xmldoc.SelectSingleNode("//emailaddress").InnerText;
				_MerchantReference = xmldoc.SelectSingleNode("//MerchantReference").InnerText;
				_ResponseText = xmldoc.SelectSingleNode("//ResponseText").InnerText;
				_Success = xmldoc.SelectSingleNode("//Success").InnerText;
				_TxnData1 = xmldoc.SelectSingleNode("//TxnData1").InnerText;
				_TxnData2 = xmldoc.SelectSingleNode("//TxnData2").InnerText;
				_TxnData3 = xmldoc.SelectSingleNode("//TxnData3").InnerText;
				_TxnId = xmldoc.SelectSingleNode("//TxnId").InnerText;
				_TxnType = xmldoc.SelectSingleNode("//TxnType").InnerText;
				_TxnMac = xmldoc.SelectSingleNode("//TxnMac").InnerText;
				_HelpText = xmldoc.SelectSingleNode("//HelpText").InnerText;
				
			}
		}

		static protected void AddXmlElement(string name, string elementValue, StringBuilder xmlString) {
			if (!String.IsNullOrEmpty(elementValue))
			{
				string xmlToAdd = String.Format("<{0}>{1}</{0}>\r\n", name, elementValue);
				xmlString.Append(xmlToAdd);
			}
		}
	}
}