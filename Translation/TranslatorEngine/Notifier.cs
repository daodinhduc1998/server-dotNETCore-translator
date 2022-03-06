using System;
using Translation.chardet;

namespace Translation.TranslatorEngine
{
    public class Notifier : nsICharsetDetectionObserver
    {
        public void Notify(string charset)
        {
            CharsetDetector.DetectedCharset = charset;
        }
    }
}
