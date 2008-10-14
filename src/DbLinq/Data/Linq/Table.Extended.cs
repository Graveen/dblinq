﻿#region MIT license
// 
// MIT license
//
// Copyright (c) 2007-2008 Jiri Moudry, Pascal Craponne
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
#endregion

using System.Collections.Generic;
using System.Linq;

#if MONO_STRICT
namespace System.Data.Linq
#else
namespace DbLinq.Data.Linq
#endif
{
    /// <summary>
    /// T may be eg. class Employee or string - the output
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    partial class Table<TEntity>
    {
        public void CancelDeleteOnSubmit(TEntity entity)
        {
            Context.UnregisterDelete(entity);
        }

        void ITable.CancelDeleteOnSubmit(object entity)
        {
            Context.UnregisterDelete(entity);
        }

        public int BulkInsertPageSize { get; set; }

        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            BulkInsert(entities, BulkInsertPageSize);
        }

        public void BulkInsert(IEnumerable<TEntity> entities, int pageSize)
        {
            using (Context.DatabaseContext.OpenConnection())
            using (var transaction = Context.DatabaseContext.Transaction())
            {
                Context.Vendor.BulkInsert(this, entities.ToList(), pageSize, transaction.Transaction);
                transaction.Commit();
            }
        }
    }
}
