using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Common.Utilities
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public static class EncryptionHelper
    {

        #region 对称/非对称

        /// <summary>
        /// 对称加密
        /// </summary>
        /// <param name="encryptType">加密类型</param>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="ivString">初始化向量</param>
        /// <param name="keyString">加密密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string SymmetricEncrypt(SymmetricEncryptType encryptType, string str, string ivString, string keyString)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(ivString) || string.IsNullOrEmpty(keyString))
                return str;

            SymmetricEncrypt encrypt = new SymmetricEncrypt(encryptType);
            encrypt.IVString = ivString;
            encrypt.KeyString = keyString;
            return encrypt.Encrypt(str);
        }

        /// <summary>
        /// 对称解密
        /// </summary>
        /// <param name="encryptType">加密类型</param>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="ivString">初始化向量</param>
        /// <param name="keyString">加密密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string SymmetricDncrypt(SymmetricEncryptType encryptType, string str, string ivString, string keyString)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            SymmetricEncrypt encrypt = new SymmetricEncrypt(encryptType);
            encrypt.IVString = ivString;
            encrypt.KeyString = keyString;
            return encrypt.Decrypt(str);
        }

        #endregion

        #region MD5

        /// <summary>
        /// 标准MD5加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// 16位的MD5加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5_16(string str)
        {
            return MD5(str).Substring(8, 16);
        }

        #endregion

        #region base64编码/解码

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">待编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string Base64_Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="str">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string Base64_Decode(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        #endregion
    }

    /// <summary>
    /// 对称加密算法
    /// </summary>
    public class SymmetricEncrypt
    {
        private SymmetricEncryptType _mbytEncryptionType;
        private string _mstrOriginalString;
        private string _mstrEncryptedString;
        private SymmetricAlgorithm _mCSP;

        #region "Constructors"

        /// <summary>
        /// 默认采用DES算法
        /// </summary>
        public SymmetricEncrypt()
        {
            _mbytEncryptionType = SymmetricEncryptType.DES;
            this.SetEncryptor();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encryptionType">加密类型</param>
        public SymmetricEncrypt(SymmetricEncryptType encryptionType)
        {
            _mbytEncryptionType = encryptionType;
            this.SetEncryptor();
        }

        /// <summary>
        /// 构造行数
        /// </summary>
        /// <param name="encryptionType">加密类型</param>
        /// <param name="originalString">原始字符串</param>
        public SymmetricEncrypt(SymmetricEncryptType encryptionType, string originalString)
        {
            _mbytEncryptionType = encryptionType;
            _mstrOriginalString = originalString;
            this.SetEncryptor();
        }

        #endregion

        #region "Public Properties"

        /// <summary>
        /// 加密类型
        /// </summary>
        public SymmetricEncryptType EncryptionType
        {
            get { return _mbytEncryptionType; }
            set
            {
                if (_mbytEncryptionType != value)
                {
                    _mbytEncryptionType = value;
                    _mstrOriginalString = String.Empty;
                    _mstrEncryptedString = String.Empty;

                    this.SetEncryptor();
                }
            }
        }

        /// <summary>
        /// 对称加密算法提供者
        /// </summary>
        public SymmetricAlgorithm CryptoProvider
        {
            get { return _mCSP; }
            set { _mCSP = value; }
        }

        /// <summary>
        /// 原始字符串
        /// </summary>
        public string OriginalString
        {
            get { return _mstrOriginalString; }
            set { _mstrOriginalString = value; }
        }

        /// <summary>
        /// 加密后的字符
        /// </summary>
        public string EncryptedString
        {
            get { return _mstrEncryptedString; }
            set { _mstrEncryptedString = value; }
        }

        /// <summary>
        /// 对称加密算法密钥
        /// </summary>
        public byte[] key
        {
            get { return _mCSP.Key; }
            set { _mCSP.Key = value; }
        }

        /// <summary>
        /// 加密密钥
        /// </summary>
        public string KeyString
        {
            get { return Convert.ToBase64String(_mCSP.Key); }
            set { _mCSP.Key = Convert.FromBase64String(value); }
        }

        /// <summary>
        /// 初始化向量
        /// </summary>
        public byte[] IV
        {
            get { return _mCSP.IV; }
            set { _mCSP.IV = value; }
        }

        /// <summary>
        /// 初始化向量(Base64)
        /// </summary>
        public string IVString
        {
            get { return Convert.ToBase64String(_mCSP.IV); }
            set { _mCSP.IV = Convert.FromBase64String(value); }
        }

        #endregion

        #region "Encrypt() Methods"

        /// <summary>
        /// 进行对称加密
        /// </summary>
        public string Encrypt()
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = _mCSP.CreateEncryptor(_mCSP.Key, _mCSP.IV);

            byt = Encoding.Unicode.GetBytes(_mstrOriginalString);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            _mstrEncryptedString = Convert.ToBase64String(ms.ToArray());
            return _mstrEncryptedString;
        }

        /// <summary>
        /// 进行对称加密
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        public string Encrypt(string originalString)
        {
            _mstrOriginalString = originalString;

            return this.Encrypt();
        }

        /// <summary>
        /// 进行对称加密
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        /// <param name="encryptionType">加密类型</param>
        public string Encrypt(string originalString, SymmetricEncryptType encryptionType)
        {
            _mstrOriginalString = originalString;
            _mbytEncryptionType = encryptionType;

            return this.Encrypt();
        }

        #endregion

        #region "Decrypt() Methods"

        /// <summary>
        /// 进行对称解密
        /// </summary>
        public string Decrypt()
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = _mCSP.CreateDecryptor(_mCSP.Key, _mCSP.IV);

            byt = Convert.FromBase64String(_mstrEncryptedString);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();

            _mstrOriginalString = Encoding.Unicode.GetString(ms.ToArray());

            return _mstrOriginalString;
        }

        /// <summary>
        /// 进行对称解密
        /// </summary>
        /// <param name="encryptedString">需要解密的字符串</param>
        public string Decrypt(string encryptedString)
        {
            _mstrEncryptedString = encryptedString;

            return this.Decrypt();
        }

        /// <summary>
        /// 进行对称解密
        /// </summary>
        /// <param name="encryptedString">需要解密的字符串</param>
        /// <param name="encryptionType">字符串加密类型</param>
        public string Decrypt(string encryptedString, SymmetricEncryptType encryptionType)
        {
            _mstrEncryptedString = encryptedString;
            _mbytEncryptionType = encryptionType;

            return this.Decrypt();
        }

        #endregion

        #region "SetEncryptor() Method"

        /// <summary>
        /// 设置加密算法
        /// </summary>
        private void SetEncryptor()
        {
            switch (_mbytEncryptionType)
            {
                case SymmetricEncryptType.DES:
                    _mCSP = new DESCryptoServiceProvider();
                    break;
                case SymmetricEncryptType.RC2:
                    _mCSP = new RC2CryptoServiceProvider();
                    break;
                case SymmetricEncryptType.Rijndael:
                    _mCSP = new RijndaelManaged();
                    break;
                case SymmetricEncryptType.TripleDES:
                    _mCSP = new TripleDESCryptoServiceProvider();
                    break;
            }

            // Generate Key
            _mCSP.GenerateKey();

            // Generate IV
            _mCSP.GenerateIV();
        }

        #endregion

        #region "Misc Public Methods"

        /// <summary>
        /// 生成随机密钥
        /// </summary>
        public string GenerateKey()
        {
            _mCSP.GenerateKey();
            return Convert.ToBase64String(_mCSP.Key);
        }

        /// <summary>
        /// 生成随机初始化向量
        /// </summary>
        public string GenerateIV()
        {
            _mCSP.GenerateIV();
            return Convert.ToBase64String(_mCSP.IV);
        }

        #endregion
    }

    /// <summary>
    /// 对称加密类型
    /// </summary>
    public enum SymmetricEncryptType : byte
    {
        /// <summary>
        /// DES算法
        /// </summary>
        DES,
        /// <summary>
        ///RC2算法 
        /// </summary>
        RC2,
        /// <summary>
        /// Rijndael算法
        /// </summary>
        Rijndael,
        /// <summary>
        /// TripleDES算法
        /// </summary>
        TripleDES
    }
}
