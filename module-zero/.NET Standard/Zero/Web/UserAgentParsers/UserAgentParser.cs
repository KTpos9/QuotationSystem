using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Zero.Web.UserAgentParsers
{
    public class UserAgentParser
    {
        private readonly Dictionary<string, string> patterns;

        public UserAgentParser()
        {
            patterns = new Dictionary<string, string>();
            // find more patterns here: https://jonlabelle.com/snippets/view/yaml/browser-user-agent-regular-expressions

            // Edge (Edg/81.0.416.72)
            patterns.Add(@"(Edg|Edge)/(\d+)\.(\d+)(?:\.(\d+))?", "Edge");

            // Chrome Mobile
            patterns.Add(@"; wv\).+(Chrome)/(\d+)\.(\d+)\.(\d+)\.(\d+)", "Chrome Mobile WebView");
            patterns.Add(@"(CrMo)/(\d+)\.(\d+)\.(\d+)\.(\d+)", "Chrome Mobile");
            patterns.Add(@"(CriOS)/(\d+)\.(\d+)\.(\d+)\.(\d+)", "Chrome Mobile iOS");
            patterns.Add(@"(Chrome)/(\d+)\.(\d+)\.(\d+)\.(\d+) Mobile(?:[ /]|$)", "Chrome Mobile");
            patterns.Add(@" Mobile .*(Chrome)/(\d+)\.(\d+)\.(\d+)\.(\d+)", "Chrome Mobile");
            // Chrome
            patterns.Add(@"(Chromium|Chrome)/(\d+)\.(\d+)(?:\.(\d+))?", "Chrome");
            // Firefox Mobile
            patterns.Add(@"(Fennec)/(\d+)\.(\d+)\.?([ab]?\d+[a-z]*)", "Firefox Mobile");
            patterns.Add(@"(Fennec)/(\d+)\.(\d+)(pre)", "Firefox Mobile");
            patterns.Add(@"(Fennec)/(\d+)\.(\d+)", "Firefox Mobile");
            patterns.Add(@"(?:Mobile|Tablet);.*(Firefox)/(\d+)\.(\d+)", "Firefox Mobile");
            patterns.Add(@"(FxiOS)/(\d+)\.(\d+)(\.(\d+))?(\.(\d+))?", "Firefox iOS");
            // Firefox
            patterns.Add(@"(Firefox)/(\d+)\.(\d+)\.(\d+)", "");
            patterns.Add(@"(Firefox)/(\d+)\.(\d+)(pre|[ab]\d+[a-z]*)?", "");
            // Safari Mobile
            patterns.Add(@"(iPod|iPhone|iPad).+Version/(\d+)\.(\d+)(?:\.(\d+))?.*[ +]Safari", "Mobile Safari");
            patterns.Add(@"(iPod|iPhone|iPad).+Version/(\d+)\.(\d+)(?:\.(\d+))?", "Mobile Safari UI/WKWebView");
            patterns.Add(@"(iPod|iPod touch|iPhone|iPad);.*CPU.*OS[ +](\d+)_(\d+)(?:_(\d+))?.*Mobile.*[ +]Safari", "Mobile Safari");
            patterns.Add(@"(iPod|iPod touch|iPhone|iPad);.*CPU.*OS[ +](\d+)_(\d+)(?:_(\d+))?.*Mobile", "Mobile Safari UI/WKWebView");
            patterns.Add(@"(iPod|iPhone|iPad).* Safari", "Mobile Safari");
            patterns.Add(@"(iPod|iPhone|iPad)", "Mobile Safari UI/WKWebView");
            // Safari
            patterns.Add(@"(Version)/(\d+)\.(\d+)(?:\.(\d+))?.*Safari/", "Safari");
            patterns.Add(@"(Safari)/\d+", "Safari");
            // IE Mobile
            patterns.Add(@"(IEMobile)[ /](\d+)\.(\d+)", "IE Mobile");
            patterns.Add(@"(MSIE) (\d+)\.(\d+).*XBLWP7", "IE Large Screen");
            // IE
            patterns.Add(@"Trident(.*)rv.(\d+)\.(\d+)", "IE");
            patterns.Add(@"([MS]?IE) (\d+)\.(\d+)", "IE");
            // Edge
            patterns.Add(@"Windows Phone .*(Edge)/(\d+)\.(\d+)", "Edge Mobile");
            patterns.Add(@"(Edge)/(\d+)(?:\.(\d+))?", "");
            // Opera
            patterns.Add(@"(Opera Tablet).*Version/(\d+)\.(\d+)(?:\.(\d+))?", "");
            patterns.Add(@"(Opera Mini)(?:/att)?/?(\d+)?(?:\.(\d+))?(?:\.(\d+))?", "");
            patterns.Add(@"(Opera)/.+Opera Mobi.+Version/(\d+)\.(\d+)", "Opera Mobile");
            patterns.Add(@"(Opera)/(\d+)\.(\d+).+Opera Mobi", "Opera Mobile");
            patterns.Add(@"Opera Mobi.+(Opera)(?:/|\s+)(\d+)\.(\d+)", "Opera Mobile");
            patterns.Add(@"Opera Mobi", "Opera Mobile");
            patterns.Add(@"(Opera)/9.80.*Version/(\d+)\.(\d+)(?:\.(\d+))?", "");
            patterns.Add(@"(?:Mobile Safari).*(OPR)/(\d+)\.(\d+)\.(\d+)", "Opera Mobile");
            patterns.Add(@"(?:Chrome).*(OPR)/(\d+)\.(\d+)\.(\d+)", "Opera");
            patterns.Add(@"(Coast)/(\d+).(\d+).(\d+)", "Opera Coast");
            patterns.Add(@"(OPiOS)/(\d+).(\d+).(\d+)", "Opera Mini");
            patterns.Add(@"Chrome/.+( MMS)/(\d+).(\d+).(\d+)", "Opera Neon");

            // Social Networks
            patterns.Add(@"\[FB.*;(FBAV)/(\d+)(?:\.(\d+)(?:\.(\d+))?)?", "Facebook");
            patterns.Add(@"\[(Pinterest)/[^\]]+\]", "Pinterest");
            patterns.Add(@"(Pinterest)(?: for Android(?: Tablet)?)?/(\d+)(?:\.(\d+)(?:\.(\d+))?)?", "Pinterest");
            // Others
            patterns.Add(@"\b(MobileIron|FireWeb|Jasmine|ANTGalio|Midori|Fresco|Lobo|PaleMoon|Maxthon|Lynx|OmniWeb|Dillo|Camino|Demeter|Fluid|Fennec|Epiphany|Shiira|Sunrise|Spotify|Flock|Netscape|Lunascape|WebPilot|NetFront|Netfront|Konqueror|SeaMonkey|Kazehakase|Vienna|Iceape|Iceweasel|IceWeasel|Iron|K-Meleon|Sleipnir|Galeon|GranParadiso|Opera Mini|iCab|NetNewsWire|ThunderBrowse|Iris|UP\.Browser|Bunjalloo|Google Earth|Raven for Mac|Openwave|MacOutlook|Electron)/(\d+)\.(\d+)\.(\d+)", "");
            patterns.Add(@"(bingbot|Bolt|AdobeAIR|Jasmine|IceCat|Skyfire|Midori|Maxthon|Lynx|Arora|IBrowse|Dillo|Camino|Shiira|Fennec|Phoenix|Flock|Netscape|Lunascape|Epiphany|WebPilot|Opera Mini|Opera|NetFront|Netfront|Konqueror|Googlebot|SeaMonkey|Kazehakase|Vienna|Iceape|Iceweasel|IceWeasel|Iron|K-Meleon|Sleipnir|Galeon|GranParadiso|iCab|iTunes|MacAppStore|NetNewsWire|Space Bison|Stainless|Orca|Dolfin|BOLT|Minimo|Tizen Browser|Polaris|Abrowser|Planetweb|ICE Browser|mDolphin|qutebrowser|Otter|QupZilla|MailBar|kmail2|YahooMobileMail|ExchangeWebServices|ExchangeServicesClient|Dragon|Outlook-iOS-Android)/(\d+)\.(\d+)(?:\.(\d+))?", "");
        }

        public BrowserVersion Parse(string userAgent)
        {
            try
            {
                foreach (var pattern in patterns)
                {
                    Regex regex = new Regex(pattern.Key);
                    Match match = regex.Match(userAgent);
                    if (match.Success)
                    {
                        return GetBrowserVersion(pattern, match);
                    }
                }

                return new BrowserVersion("Unknown", "", "", "");
            }
            catch (Exception)
            {
                return new BrowserVersion("Unknown", "", "", "");
            }
        }

        private BrowserVersion GetBrowserVersion(KeyValuePair<string, string> pattern, Match match)
        {
            var browser = pattern.Value;
            var major = "";
            var minor = "";
            var patch = "";
            var groups = match.Groups.Count;
            if (string.IsNullOrEmpty(browser) && groups > 1)
            {
                browser = match.Groups[1].Value;
            }
            // major start from index 2
            major = GetMajor(match, major, groups);
            minor = GetMinor(match, minor, groups);
            patch = GetPatch(match, patch, groups);

            return new BrowserVersion(browser, major, minor, patch);
        }

        private string GetMajor(Match match, string major, int groups)
        {
            if (groups > 2)
            {
                major = match.Groups[2].Value;
            }

            return major;
        }

        private string GetMinor(Match match, string minor, int groups)
        {
            if (groups > 3)
            {
                minor = match.Groups[3].Value;
            }

            return minor;
        }

        private string GetPatch(Match match, string patch, int groups)
        {
            if (groups > 4)
            {
                patch = match.Groups[4].Value;
            }

            return patch;
        }
    }
}