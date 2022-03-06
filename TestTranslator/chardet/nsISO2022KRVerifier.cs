/* ***** BEGIN LICENSE BLOCK *****
* Version: MPL 1.1/GPL 2.0/LGPL 2.1
*
* The contents of this file are subject to the Mozilla Public License Version
* 1.1 (the "License"); you may not use this file except in compliance with
* the License. You may obtain a copy of the License at
* http://www.mozilla.org/MPL/
*
* Software distributed under the License is distributed on an "AS IS" basis,
* WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
* for the specific language governing rights and limitations under the
* License.
*
* The Original Code is mozilla.org code.
*
* The Initial Developer of the Original Code is
* Netscape Communications Corporation.
* Portions created by the Initial Developer are Copyright (C) 1998
* the Initial Developer. All Rights Reserved.
*
* Contributor(s):
*   Craig Dunn <craig dot dunn at conceptdevelopment dot net>
*
* Alternatively, the contents of this file may be used under the terms of
* either of the GNU General Public License Version 2 or later (the "GPL"),
* or the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
* in which case the provisions of the GPL or the LGPL are applicable instead
* of those above. If you wish to allow use of your version of this file only
* under the terms of either the GPL or the LGPL, and not to allow others to
* use your version of this file under the terms of the MPL, indicate your
* decision by deleting the provisions above and replace them with the notice
* and other provisions required by the GPL or the LGPL. If you do not delete
* the provisions above, a recipient may use your version of this file under
* the terms of any one of the MPL, the GPL or the LGPL.
*
* ***** END LICENSE BLOCK ***** */
/* 
 * DO NOT EDIT THIS DOCUMENT MANUALLY !!!
 * THIS FILE IS AUTOMATICALLY GENERATED BY THE TOOLS UNDER
 *    AutoDetect/tools/
 */
using System;

namespace Translator.chardet
{

    //import java.lang.* ;

    public class nsISO2022KRVerifier : nsVerifier
    {

        static int[] m_cclass;
        static int[] m_states;
        static int m_stFactor;
        static string m_charset;

        public override int[] cclass() { return m_cclass; }
        public override int[] states() { return m_states; }
        public override int stFactor() { return m_stFactor; }
        public override string charset() { return m_charset; }

        public nsISO2022KRVerifier()
        {

            m_cclass = new int[256 / 8];

            m_cclass[0] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 2;
            m_cclass[1] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[2] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[3] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (1 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[4] = ((0 << 4 | 0) << 8 | 0 << 4 | 3) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[5] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 4 << 4 | 0;
            m_cclass[6] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[7] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[8] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (5 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[9] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[10] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[11] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[12] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[13] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[14] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[15] = ((0 << 4 | 0) << 8 | 0 << 4 | 0) << 16 | (0 << 4 | 0) << 8 | 0 << 4 | 0;
            m_cclass[16] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[17] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[18] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[19] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[20] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[21] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[22] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[23] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[24] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[25] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[26] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[27] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[28] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[29] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[30] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;
            m_cclass[31] = ((2 << 4 | 2) << 8 | 2 << 4 | 2) << 16 | (2 << 4 | 2) << 8 | 2 << 4 | 2;



            m_states = new int[5];

            m_states[0] = ((eError << 4 | eError) << 8 | eStart << 4 | eStart) << 16 | (eStart << 4 | eError) << 8 | 3 << 4 | eStart;
            m_states[1] = ((eItsMe << 4 | eItsMe) << 8 | eItsMe << 4 | eItsMe) << 16 | (eError << 4 | eError) << 8 | eError << 4 | eError;
            m_states[2] = ((eError << 4 | eError) << 8 | 4 << 4 | eError) << 16 | (eError << 4 | eError) << 8 | eItsMe << 4 | eItsMe;
            m_states[3] = ((eError << 4 | eError) << 8 | eError << 4 | 5) << 16 | (eError << 4 | eError) << 8 | eError << 4 | eError;
            m_states[4] = ((eStart << 4 | eStart) << 8 | eStart << 4 | eStart) << 16 | (eItsMe << 4 | eError) << 8 | eError << 4 | eError;



            m_charset = "ISO-2022-KR";
            m_stFactor = 6;

        }

        public override bool isUCS2() { return false; }


    }


} // namespace