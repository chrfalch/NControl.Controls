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
		public const string MD3drotation = "\uf000";
		public const string MDaccessibility = "\uf001";		
		public const string MDaccountbalance = "\uf002";
		public const string MDaccountbalancewallet = "\uf003";
		public const string MDaccountbox = "\uf004";
		public const string MDaccountchild = "\uf005";
		public const string MDaccountcircle = "\uf006";
		public const string addshoppingcart = "\uf007";
		public const string alarm = "\uf008";
		public const string alarmadd = "\uf009";
		public const string alarmoff = "\uf00a";
		public const string alarmon = "\uf00b";
		public const string android = "\uf00c";
		public const string announcement = "\uf00d";
		public const string aspectratio = "\uf00e";
		public const string assessment = "\uf00f";
		public const string assignment = "\uf010";
		public const string assignmentind = "\uf011";
		public const string assignmentlate = "\uf012";
		public const string assignmentreturn= "\uf013";
		public const string assignmentreturned= "\uf014";
		public const string assignmentturnedin= "\uf015";
		public const string autorenew= "\uf016";
		public const string backup= "\uf017";
		public const string book= "\uf018";
		public const string bookmark= "\uf019";
		public const string bookmarkoutline= "\uf01a";
		public const string bugreport= "\uf01b";
		public const string cached= "\uf01c";
		public const string mclass= "\uf01d";
		public const string creditcard= "\uf01e";
		public const string dashboard= "\uf01f";
		public const string delete= "\uf020";
		public const string description= "\uf021";
		public const string dns= "\uf022";
		public const string done= "\uf023";
		public const string doneall= "\uf024";
		public const string mevent= "\uf025";
		public const string exittoapp= "\uf026";
		public const string explore= "\uf027";
		public const string extension= "\uf028";
		public const string faceunlock= "\uf029";
		public const string favorite= "\uf02a";
		public const string favoriteoutline= "\uf02b";
		public const string findinpage= "\uf02c";
		public const string findreplace= "\uf02d";
		public const string fliptoback= "\uf02e";
		public const string fliptofront= "\uf02f";
		public const string getapp= "\uf030";
		public const string grade= "\uf031";
		public const string groupwork= "\uf032";
		public const string help= "\uf033";
		public const string highlightremove= "\uf034";
		public const string history= "\uf035";
		public const string home= "\uf036";
		public const string https= "\uf037";
		public const string info= "\uf038";
		public const string infooutline= "\uf039";
		public const string input= "\uf03a";
		public const string invertcolors= "\uf03b";
		public const string label= "\uf03c";
		public const string labeloutline= "\uf03d";
		public const string language= "\uf03e";
		public const string launch= "\uf03f";
		public const string list= "\uf040";
		public const string mlock = "\uf041";
		public const string lockopen= "\uf042";
		public const string lockoutline= "\uf043";
		public const string loyalty= "\uf044";
		public const string markunreadmailbox= "\uf045";
		public const string noteadd= "\uf046";
		public const string openinbrowser= "\uf047";
		public const string openinnew= "\uf048";
		public const string openwith= "\uf049";
		public const string pageview= "\uf04a";
		public const string payment= "\uf04b";
		public const string permcameramic= "\uf04c";
		public const string permcontactcal= "\uf04d";
		public const string permdatasetting= "\uf04e";
		public const string permdeviceinfo= "\uf04f";
		public const string permidentity= "\uf050";
		public const string permmedia= "\uf051";
		public const string permphonemsg= "\uf052";
		public const string permscanwifi= "\uf053";
		public const string pictureinpicture= "\uf054";
		public const string polymer= "\uf055";
		public const string print= "\uf056";
		public const string querybuilder= "\uf057";
		public const string questionanswer= "\uf058";
		public const string receipt= "\uf059";
		public const string redeem= "\uf05a";
		public const string reportproblem= "\uf05b";
		public const string restore= "\uf05c";
		public const string room= "\uf05d";
		public const string schedule= "\uf05e";
		public const string search= "\uf05f";
		public const string settings= "\uf060";
		public const string settingsapplications= "\uf061";
		public const string settingsbackuprestore= "\uf062";
		public const string settingsbluetooth= "\uf063";
		public const string settingscell= "\uf064";
		public const string settingsdisplay= "\uf065";
		public const string settingsethernet= "\uf066";
		public const string settingsinputantenna= "\uf067";
		public const string settingsinputcomponent= "\uf068";
		public const string settingsinputcomposite= "\uf069";
		public const string settingsinputhdmi= "\uf06a";
		public const string settingsinputsvideo= "\uf06b";
		public const string settingsoverscan= "\uf06c";
		public const string settingsphone= "\uf06d";
		public const string settingspower= "\uf06e";
		public const string settingsremote= "\uf06f";
		public const string settingsvoice= "\uf070";
		public const string shop= "\uf071";
		public const string shoppingbasket= "\uf072";
		public const string shoppingcart= "\uf073";
		public const string shoptwo= "\uf074";
		public const string speakernotes= "\uf075";
		public const string spellcheck= "\uf076";
		public const string starrate= "\uf077";
		public const string stars= "\uf078";
		public const string store= "\uf079";
		public const string subject= "\uf07a";
		public const string swaphoriz= "\uf07b";
		public const string swapvert= "\uf07c";
		public const string swapvertcircle= "\uf07d";
		public const string systemupdatetv= "\uf07e";
		public const string tab= "\uf07f";
		public const string tabunselected= "\uf080";
		public const string theaters= "\uf081";
		public const string thumbdown= "\uf082";
		public const string thumbsupdown= "\uf083";
		public const string thumbup= "\uf084";
		public const string toc= "\uf085";
		public const string today= "\uf086";
		public const string trackchanges= "\uf087";
		public const string translate= "\uf088";
		public const string trendingdown= "\uf089";
		public const string trendingneutral= "\uf08a";
		public const string trendingup= "\uf08b";
		public const string turnedin= "\uf08c";
		public const string turnedinnot= "\uf08d";
		public const string verifieduser= "\uf08e";
		public const string viewagenda= "\uf08f";
		public const string viewarray= "\uf090";
		public const string viewcarousel= "\uf091";
		public const string viewcolumn= "\uf092";
		public const string viewday= "\uf093";
		public const string viewheadline= "\uf094";
		public const string viewlist= "\uf095";
		public const string viewmodule= "\uf096";
		public const string viewquilt= "\uf097";
		public const string viewstream= "\uf098";
		public const string viewweek= "\uf099";
		public const string visibility= "\uf09a";
		public const string visibilityoff= "\uf09b";
		public const string walletgiftcard= "\uf09c";
		public const string walletmembership= "\uf09d";
		public const string wallettravel= "\uf09e";
		public const string work= "\uf09f";

		// Alerts
		public const string error = "\uf0a0";
		public const string warning = "\uf0a1";

		// Video
		public const string album = "\uf0a2";
		public const string avtimer = "\uf0a3";
		public const string closedcaption = "\uf0a4";
		public const string equalizer = "\uf0a5";
		public const string mexplicit = "\uf0a6";
		public const string fastforward = "\uf0a7";
		public const string fastrewind = "\uf0a8";
		public const string games = "\uf0a9";
		public const string hearing = "\uf0aa";
		public const string highquality = "\uf0ab";
		public const string loop = "\uf0ac";
		public const string mic = "\uf0ad";
		public const string micnone = "\uf0ae";
		public const string micoff = "\uf0af";
		public const string movie = "\uf0b0";
		public const string mylibraryadd = "\uf0b1";
		public const string mylibrarybooks = "\uf0b2";
		public const string mylibrarymusic = "\uf0b3";
		public const string newreleases = "\uf0b4";
		public const string notinterested = "\uf0b5";
		public const string pause = "\uf0b6";
		public const string pausecirclefill = "\uf0b7";
		public const string pausecircleoutline = "\uf0b8";
		public const string playarrow = "\uf0b9";
		public const string playcirclefill = "\uf0ba";
		public const string playcircleoutline = "\uf0bb";
		public const string playlistadd = "\uf0bc";
		public const string playshoppingbag = "\uf0bd";
		public const string queue = "\uf0be";
		public const string queuemusic = "\uf0bf";
		public const string radio = "\uf0c0";
		public const string recentactors = "\uf0c1";
		public const string repeat = "\uf0c2";
		public const string repeatone = "\uf0c3";
		public const string replay = "\uf0c4";
		public const string shuffle = "\uf0c5";
		public const string skipnext = "\uf0c6";
		public const string skipprevious = "\uf0c7";
		public const string snooze = "\uf0c8";
		public const string stop = "\uf0c9";
		public const string subtitles = "\uf0ca";
		public const string surroundsound = "\uf0cb";
		public const string videocam = "\uf0cc";
		public const string videocamoff = "\uf0cd";
		public const string videocollection = "\uf0ce";
		public const string volumedown = "\uf0cf";
		public const string volumemute = "\uf0d0";
		public const string volumeoff = "\uf0d1";
		public const string volumeup = "\uf0d2";
		public const string web = "\uf0d3";

		// Communications
		public const string business="\uf0d4";
		public const string call="\uf0d5";
		public const string callend="\uf0d6";
		public const string callmade="\uf0d7";
		public const string callmerge="\uf0d8";
		public const string callmissed="\uf0d9";
		public const string callreceived="\uf0da";
		public const string callsplit="\uf0db";
		public const string chat="\uf0dc";
		public const string clearall="\uf0dd";
		public const string comment="\uf0de";
		public const string contacts="\uf0df";
		public const string dialersip="\uf0e0";
		public const string dialpad="\uf0e1";
		public const string dndon="\uf0e2";
		public const string email="\uf0e3";
		public const string forum="\uf0e4";
		public const string importexport="\uf0e5";
		public const string invertcolorsoff="\uf0e6";
		public const string invertcolorson="\uf0e7";
		public const string livehelp="\uf0e8";
		public const string locationoff="\uf0e9";
		public const string locationon="\uf0ea";
		public const string message="\uf0eb";
		public const string messenger="\uf0ec";
		public const string nosim="\uf0ed";
		public const string phone="\uf0ee";
		public const string portablewifioff="\uf0ef";
		public const string quickcontactsdialer="\uf0f0";
		public const string quickcontactsmail="\uf0f1";
		public const string ringvolume="\uf0f2";
		public const string staycurrentlandscape="\uf0f3";
		public const string staycurrentportrait="\uf0f4";
		public const string stayprimarylandscape="\uf0f5";
		public const string stayprimaryportrait="\uf0f6";
		public const string swapcalls="\uf0f7";
		public const string textsms="\uf0f8";
		public const string voicemail="\uf0f9";
		public const string vpnkey="\uf0fa";

		// Content
		// Device
		// File
		// Hardware
		// Image

		// Maps
		public const string beenhere = "\uf25b";
		public const string directions = "\uf25c";
		public const string directionsbike = "\uf25d";
		public const string directionsbus = "\uf25e";
		public const string directionscar = "\uf25f";
		public const string directionsferry = "\uf260";
		public const string directionssubway = "\uf261";
		public const string directionstrain = "\uf262";
		public const string directionstransit = "\uf263";
		public const string directionswalk = "\uf264";
		public const string flight = "\uf265";
		public const string hotel = "\uf266";
		public const string layers = "\uf267";
		public const string layersclear = "\uf268";
		public const string localairport = "\uf269";
		public const string localatm = "\uf26a";
		public const string localattraction = "\uf26b";
		public const string localbar = "\uf26c";
		public const string localcafe = "\uf26d";
		public const string localcarwash = "\uf26e";
		public const string localconveniencestore = "\uf26f";
		public const string localdrink = "\uf270";
		public const string localflorist = "\uf271";
		public const string localgasstation = "\uf272";
		public const string localgrocerystore = "\uf273";
		public const string localhospital = "\uf274";
		public const string localhotel = "\uf275";
		public const string locallaundryservice = "\uf276";
		public const string locallibrary = "\uf277";
		public const string localmall = "\uf278";
		public const string localmovies = "\uf279";
		public const string localoffer = "\uf27a";
		public const string localparking = "\uf27b";
		public const string localpharmacy = "\uf27c";
		public const string localphone = "\uf27d";
		public const string localpizza = "\uf27e";
		public const string localplay = "\uf27f";
		public const string localpostoffice = "\uf280";
		public const string localprintshop = "\uf281";
		public const string localrestaurant = "\uf282";
		public const string localsee = "\uf283";
		public const string localshipping = "\uf284";
		public const string localtaxi = "\uf285";
		public const string locationhistory = "\uf286";
		public const string map = "\uf287";
		public const string mylocation = "\uf288";
		public const string navigation = "\uf289";
		public const string pindrop = "\uf28a";
		public const string place = "\uf28b";
		public const string ratereview = "\uf28c";
		public const string restaurantmenu = "\uf28d";
		public const string satellite = "\uf28e";
		public const string storemalldirectory = "\uf28f";
		public const string terrain = "\uf290";
		public const string traffic = "\uf291";


	}
}

