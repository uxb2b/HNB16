using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using ModelCore.Locale;
using CommonLib.Utility;
using CommonLib.DataAccess;
using System.Data.Linq;
using ModelCore.UserManagement;

namespace ModelCore.UploadManagement
{
    public abstract class TextArrayUploadManager<T, TEntity, TextArray> : GenericManager<T, TEntity>, ICsvUploadManager
        where T : DataContext, new()
        where TEntity : class,new()
        where TextArray : struct
    {
        protected List<TextArrayUpload<TEntity,TextArray>> _items;
        protected List<TextArrayUpload<TEntity, TextArray>> _errorItems;
        protected UserProfile _userProfile;
        protected bool _bResult = false;
        protected bool _breakParsing = false;
        protected Encoding _encoding = Encoding.Default;

        public TextArrayUploadManager() : base() {
            initialize();
        }
        public TextArrayUploadManager(GenericManager<T> manager)
            : base(manager)
        {
            initialize();
        }

        protected virtual void initialize()
        {

        }


        public virtual void ParseData(UserProfile userProfile, String fileName, Encoding encoding)
        {
            _userProfile = userProfile;
            _bResult = false;
            _encoding = encoding;

            using (FileStream sr = new FileStream(fileName,FileMode.Open,FileAccess.Read))
            {
                _items = new List<TextArrayUpload<TEntity, TextArray>>();
                _bResult = true;

                int lineIdx = 0;
                var items = sr.Parse<TextArray>(encoding);
                foreach (TextArray line in items)
                {
                    lineIdx++;
                    TextArrayUpload<TEntity, TextArray> item = new TextArrayUpload<TEntity,TextArray>();
                    item.DataContent = line;

                    validate(item);

                    if (String.IsNullOrEmpty(item.Status))
                    {
                        item.UploadStatus = Naming.UploadStatusDefinition.等待匯入;
                    }
                    else
                    {
                        item.UploadStatus = Naming.UploadStatusDefinition.資料錯誤;
                        item.Status = String.Format("第{0}筆:{1}", lineIdx, item.Status.Substring(1));
                    }
                    _items.Add(item);

                    if (_breakParsing)
                        break;
                }
            }

            if (!IsValid)
            {
                _errorItems = _items.Where(i => !String.IsNullOrEmpty(i.Status)).ToList();
            }
        }

        public bool IsValid
        {
            get
            {
                return _bResult;
            }
        }

        public List<TextArrayUpload<TEntity, TextArray>> ItemList
        {
            get
            {
                return _items;
            }
        }

        public List<TextArrayUpload<TEntity, TextArray>> ErrorList
        {
            get
            {
                return _errorItems;
            }
        }


        public int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }

        protected abstract void doSave();

        public bool Save()
        {
            if (_bResult)
            {
                doSave();
                foreach (var item in _items)
                {
                    item.UploadStatus = Naming.UploadStatusDefinition.匯入成功;
                }
                return true;
            }
            return false;
        }

        protected abstract bool validate(TextArrayUpload<TEntity, TextArray> item);

    }

    public interface ITextArrayUpload<TArray>
        where TArray : struct
    {
        TArray DataContent { get; set; }
        string Status { get; set; }
        Naming.UploadStatusDefinition UploadStatus { get; set; }
    }

    public class TextArrayUpload<TEntity, TArray> : ITextArrayUpload<TArray>
        where TEntity : class,new()
        where TArray : struct
    {
        public TEntity Entity { get; set; }


        public TArray DataContent
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public Naming.UploadStatusDefinition UploadStatus
        {
            get;
            set;
        }
    }

}
