using System;
using Xamarin.Forms;

namespace NControl.Controls
{
	/// <summary>
	/// Font material design label.
	/// </summary>
	public class FontMaterialDesignLabel: Label
	{
		/// <summary>
		/// The name of the font.
		/// </summary>
		public const string FontName = "Material-Design-Iconic-Font";

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.FontMaterialDesignLabel"/> class.
		/// </summary>
		public FontMaterialDesignLabel ()
		{
			FontFamily = FontName;
			FontSize = 18;
			XAlign = TextAlignment.Center;
			YAlign = TextAlignment.Center;
		}

		// http://zavoloklom.github.io/material-design-iconic-font/cheatsheet.html
		// Actions
		public const string MD3drotation = "\u0000";
		public const string MDaccessibility = "\u0001";		
		public const string MDaccountbalance = "\u0002";
		public const string MDaccountbalancewallet = "\u0003";
		public const string MDaccountbox = "\u0004";
		public const string MDaccountchild = "\u0005";
		public const string MDaccountcircle = "\u0006";
		public const string addshoppingcart = "\u0007";
		public const string alarm = "\u0008";
		public const string alarmadd = "\u0009";
		public const string alarmoff = "\u000a";
		public const string alarmon = "\u000b";
		public const string android = "\u000c";
		public const string announcement = "\u000d";
		public const string aspectratio = "\u000e";
		public const string assessment = "\u000f";
		public const string assignment = "\u0010";
		public const string assignmentind = "\u0011";
		public const string assignmentlate = "\u0012";
		public const string assignmentreturn= "\u0013";
		public const string assignmentreturned= "\u0014";
		public const string assignmentturnedin= "\u0015";
		public const string autorenew= "\u0016";
		public const string backup= "\u0017";
		public const string book= "\u0018";
		public const string bookmark= "\u0019";
		public const string bookmarkoutline= "\u001a";
		public const string bugreport= "\u001b";
		public const string cached= "\u001c";
		public const string mclass= "\u001d";
		public const string creditcard= "\u001e";
		public const string dashboard= "\u001f";
		public const string delete= "\u0020";
		public const string description= "\u0021";
		public const string dns= "\u0022";
		public const string done= "\u0023";
		public const string doneall= "\u0024";
		public const string mevent= "\u0025";
		public const string exittoapp= "\u0026";
		public const string explore= "\u0027";
		public const string extension= "\u0028";
		public const string faceunlock= "\u0029";
		public const string favorite= "\u002a";
		public const string favoriteoutline= "\u002b";
		public const string findinpage= "\u002c";
		public const string findreplace= "\u002d";
		public const string fliptoback= "\u002e";
		public const string fliptofront= "\u002f";
		public const string getapp= "\u0030";
		public const string grade= "\u0031";
		public const string groupwork= "\u0032";
		public const string help= "\u0033";
		public const string highlightremove= "\u0034";
		public const string history= "\u0035";
		public const string home= "\u0036";
		public const string https= "\u0037";
		public const string info= "\u0038";
		public const string infooutline= "\u0039";
		public const string input= "\u003a";
		public const string invertcolors= "\u003b";
		public const string label= "\u003c";
		public const string labeloutline= "\u003d";
		public const string language= "\u003e";
		public const string launch= "\u003f";
		public const string list= "\u0040";
		public const string mlock = "\u0041";
		public const string lockopen= "\u0042";
		public const string lockoutline= "\u0043";
		public const string loyalty= "\u0044";
		public const string markunreadmailbox= "\u0045";
		public const string noteadd= "\u0046";
		public const string openinbrowser= "\u0047";
		public const string openinnew= "\u0048";
		public const string openwith= "\u0049";
		public const string pageview= "\u004a";
		public const string payment= "\u004b";
		public const string permcameramic= "\u004c";
		public const string permcontactcal= "\u004d";
		public const string permdatasetting= "\u004e";
		public const string permdeviceinfo= "\u004f";
		public const string permidentity= "\u0050";
		public const string permmedia= "\u0051";
		public const string permphonemsg= "\u0052";
		public const string permscanwifi= "\u0053";
		public const string pictureinpicture= "\u0054";
		public const string polymer= "\u0055";
		public const string print= "\u0056";
		public const string querybuilder= "\u0057";
		public const string questionanswer= "\u0058";
		public const string receipt= "\u0059";
		public const string redeem= "\u005a";
		public const string reportproblem= "\u005b";
		public const string restore= "\u005c";
		public const string room= "\u005d";
		public const string schedule= "\u005e";
		public const string search= "\u005f";
		public const string settings= "\u0060";
		public const string settingsapplications= "\u0061";
		public const string settingsbackuprestore= "\u0062";
		public const string settingsbluetooth= "\u0063";
		public const string settingscell= "\u0064";
		public const string settingsdisplay= "\u0065";
		public const string settingsethernet= "\u0066";
		public const string settingsinputantenna= "\u0067";
		public const string settingsinputcomponent= "\u0068";
		public const string settingsinputcomposite= "\u0069";
		public const string settingsinputhdmi= "\u006a";
		public const string settingsinputsvideo= "\u006b";
		public const string settingsoverscan= "\u006c";
		public const string settingsphone= "\u006d";
		public const string settingspower= "\u006e";
		public const string settingsremote= "\u006f";
		public const string settingsvoice= "\u0070";
		public const string shop= "\u0071";
		public const string shoppingbasket= "\u0072";
		public const string shoppingcart= "\u0073";
		public const string shoptwo= "\u0074";
		public const string speakernotes= "\u0075";
		public const string spellcheck= "\u0076";
		public const string starrate= "\u0077";
		public const string stars= "\u0078";
		public const string store= "\u0079";
		public const string subject= "\u007a";
		public const string swaphoriz= "\u007b";
		public const string swapvert= "\u007c";
		public const string swapvertcircle= "\u007d";
		public const string systemupdatetv= "\u007e";
		public const string tab= "\u007f";
		public const string tabunselected= "\u0080";
		public const string theaters= "\u0081";
		public const string thumbdown= "\u0082";
		public const string thumbsupdown= "\u0083";
		public const string thumbup= "\u0084";
		public const string toc= "\u0085";
		public const string today= "\u0086";
		public const string trackchanges= "\u0087";
		public const string translate= "\u0088";
		public const string trendingdown= "\u0089";
		public const string trendingneutral= "\u008a";
		public const string trendingup= "\u008b";
		public const string turnedin= "\u008c";
		public const string turnedinnot= "\u008d";
		public const string verifieduser= "\u008e";
		public const string viewagenda= "\u008f";
		public const string viewarray= "\u0090";
		public const string viewcarousel= "\u0091";
		public const string viewcolumn= "\u0092";
		public const string viewday= "\u0093";
		public const string viewheadline= "\u0094";
		public const string viewlist= "\u0095";
		public const string viewmodule= "\u0096";
		public const string viewquilt= "\u0097";
		public const string viewstream= "\u0098";
		public const string viewweek= "\u0099";
		public const string visibility= "\u009a";
		public const string visibilityoff= "\u009b";
		public const string walletgiftcard= "\u009c";
		public const string walletmembership= "\u009d";
		public const string wallettravel= "\u009e";
		public const string work= "\u009f";

		// Alerts
		public const string error = "\u00a0";
		public const string warning = "\u00a1";

		// Video
		public const string album = "\u00a2";
		public const string avtimer = "\u00a3";
		public const string closedcaption = "\u00a4";
		public const string equalizer = "\u00a5";
		public const string mexplicit = "\u00a6";
		public const string fastforward = "\u00a7";
		public const string fastrewind = "\u00a8";
		public const string games = "\u00a9";
		public const string hearing = "\u00aa";
		public const string highquality = "\u00ab";
		public const string loop = "\u00ac";
		public const string mic = "\u00ad";
		public const string micnone = "\u00ae";
		public const string micoff = "\u00af";
		public const string movie = "\u00b0";
		public const string mylibraryadd = "\u00b1";
		public const string mylibrarybooks = "\u00b2";
		public const string mylibrarymusic = "\u00b3";
		public const string newreleases = "\u00b4";
		public const string notinterested = "\u00b5";
		public const string pause = "\u00b6";
		public const string pausecirclefill = "\u00b7";
		public const string pausecircleoutline = "\u00b8";
		public const string playarrow = "\u00b9";
		public const string playcirclefill = "\u00ba";
		public const string playcircleoutline = "\u00bb";
		public const string playlistadd = "\u00bc";
		public const string playshoppingbag = "\u00bd";
		public const string queue = "\u00be";
		public const string queuemusic = "\u00bf";
		public const string radio = "\u00c0";
		public const string recentactors = "\u00c1";
		public const string repeat = "\u00c2";
		public const string repeatone = "\u00c3";
		public const string replay = "\u00c4";
		public const string shuffle = "\u00c5";
		public const string skipnext = "\u00c6";
		public const string skipprevious = "\u00c7";
		public const string snooze = "\u00c8";
		public const string stop = "\u00c9";
		public const string subtitles = "\u00ca";
		public const string surroundsound = "\u00cb";
		public const string videocam = "\u00cc";
		public const string videocamoff = "\u00cd";
		public const string videocollection = "\u00ce";
		public const string volumedown = "\u00cf";
		public const string volumemute = "\u00d0";
		public const string volumeoff = "\u00d1";
		public const string volumeup = "\u00d2";
		public const string web = "\u00d3";

		// Communications
		public const string business="\u00d4";
		public const string call="\u00d5";
		public const string callend="\u00d6";
		public const string callmade="\u00d7";
		public const string callmerge="\u00d8";
		public const string callmissed="\u00d9";
		public const string callreceived="\u00da";
		public const string callsplit="\u00db";
		public const string chat="\u00dc";
		public const string clearall="\u00dd";
		public const string comment="\u00de";
		public const string contacts="\u00df";
		public const string dialersip="\u00e0";
		public const string dialpad="\u00e1";
		public const string dndon="\u00e2";
		public const string email="\u00e3";
		public const string forum="\u00e4";
		public const string importexport="\u00e5";
		public const string invertcolorsoff="\u00e6";
		public const string invertcolorson="\u00e7";
		public const string livehelp="\u00e8";
		public const string locationoff="\u00e9";
		public const string locationon="\u00ea";
		public const string message="\u00eb";
		public const string messenger="\u00ec";
		public const string nosim="\u00ed";
		public const string phone="\u00ee";
		public const string portablewifioff="\u00ef";
		public const string quickcontactsdialer="\u00f0";
		public const string quickcontactsmail="\u00f1";
		public const string ringvolume="\u00f2";
		public const string staycurrentlandscape="\u00f3";
		public const string staycurrentportrait="\u00f4";
		public const string stayprimarylandscape="\u00f5";
		public const string stayprimaryportrait="\u00f6";
		public const string swapcalls="\u00f7";
		public const string textsms="\u00f8";
		public const string voicemail="\u00f9";
		public const string vpnkey="\u00fa";

		// Content
		// Device
		// File
		// Hardware
		// Image

		// Maps
		public const string beenhere = "\u025b";
		public const string directions = "\u025c";
		public const string directionsbike = "\u025d";
		public const string directionsbus = "\u025e";
		public const string directionscar = "\u025f";
		public const string directionsferry = "\u0260";
		public const string directionssubway = "\u0261";
		public const string directionstrain = "\u0262";
		public const string directionstransit = "\u0263";
		public const string directionswalk = "\u0264";
		public const string flight = "\u0265";
		public const string hotel = "\u0266";
		public const string layers = "\u0267";
		public const string layersclear = "\u0268";
		public const string localairport = "\u0269";
		public const string localatm = "\u026a";
		public const string localattraction = "\u026b";
		public const string localbar = "\u026c";
		public const string localcafe = "\u026d";
		public const string localcarwash = "\u026e";
		public const string localconveniencestore = "\u026f";
		public const string localdrink = "\u0270";
		public const string localflorist = "\u0271";
		public const string localgasstation = "\u0272";
		public const string localgrocerystore = "\u0273";
		public const string localhospital = "\u0274";
		public const string localhotel = "\u0275";
		public const string locallaundryservice = "\u0276";
		public const string locallibrary = "\u0277";
		public const string localmall = "\u0278";
		public const string localmovies = "\u0279";
		public const string localoffer = "\u027a";
		public const string localparking = "\u027b";
		public const string localpharmacy = "\u027c";
		public const string localphone = "\u027d";
		public const string localpizza = "\u027e";
		public const string localplay = "\u027f";
		public const string localpostoffice = "\u0280";
		public const string localprintshop = "\u0281";
		public const string localrestaurant = "\u0282";
		public const string localsee = "\u0283";
		public const string localshipping = "\u0284";
		public const string localtaxi = "\u0285";
		public const string locationhistory = "\u0286";
		public const string map = "\u0287";
		public const string mylocation = "\u0288";
		public const string navigation = "\u0289";
		public const string pindrop = "\u028a";
		public const string place = "\u028b";
		public const string ratereview = "\u028c";
		public const string restaurantmenu = "\u028d";
		public const string satellite = "\u028e";
		public const string storemalldirectory = "\u028f";
		public const string terrain = "\u0290";
		public const string traffic = "\u0291";


	}
}

