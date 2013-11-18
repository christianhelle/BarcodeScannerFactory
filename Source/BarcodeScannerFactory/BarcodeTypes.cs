#region License

// The MIT License (MIT)
// 
// Copyright (c) 2013 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion


#pragma warning disable 1591

namespace ChristianHelle.Barcode
{
    public enum BarcodeTypes
    {
        FIRST = 48,
        LAST = 48,
        UPCE0 = 48,
        NEXT = 49,
        UPCE1 = 49,
        UPCA = 50,
        MSI = 51,
        EAN8 = 52,
        EAN13 = 53,
        CODABAR = 54,
        CODE39 = 55,
        D2OF5 = 56,
        I2OF5 = 57,
        CODE11 = 58,
        CODE93 = 59,
        CODE128 = 60,
        IATA2OF5 = 62,
        EAN128 = 63,
        PDF417 = 64,
        ISBT128 = 65,
        TRIOPTIC39 = 66,
        COUPON = 67,
        BOOKLAND = 68,
        MICROPDF = 69,
        CODE32 = 70,
        MACROPDF = 71,
        MAXICODE = 72,
        DATAMATRIX = 73,
        QRCODE = 74,
        MACROMICROPDF = 75,
        RSS14 = 76,
        RSSLIM = 77,
        RSSEXP = 78,
        POINTER = 80,
        IMAGE = 81,
        SIGNATURE = 82,
        RESERVED_53 = 83,
        WEBCODE = 84,
        CUECODE = 85,
        COMPOSITE_AB = 86,
        COMPOSITE_C = 87,
        TLC39 = 88,
        USPOSTNET = 97,
        USPLANET = 98,
        UKPOSTAL = 99,
        JAPPOSTAL = 100,
        AUSPOSTAL = 101,
        DUTCHPOSTAL = 102,
        CANPOSTAL = 103,
        CHINESE_2OF5 = 112,
        AZTEC = 116,
        MICROQR = 117,
        KOREAN_3OF5 = 118,
        US4STATE = 119,
        US4STATE_FICS = 121,
        MATRIX_2OF5 = 122,
        UNKNOWN = 255,
    }
}

#pragma warning restore 1591