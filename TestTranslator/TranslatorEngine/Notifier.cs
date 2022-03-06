using System;
using Translator.chardet;

namespace Translator.TranslatorEngine
{
    public class Notifier : nsICharsetDetectionObserver
    {
        public void Notify(string charset)
        {
            CharsetDetector.DetectedCharset = charset;
        }
    }
}
