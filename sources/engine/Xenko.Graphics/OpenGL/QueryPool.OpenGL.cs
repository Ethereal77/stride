// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
#if XENKO_GRAPHICS_API_OPENGL

using System;
#if XENKO_GRAPHICS_API_OPENGLES
using OpenTK.Graphics.ES30;
#else
using OpenTK.Graphics.OpenGL;
#endif

namespace Xenko.Graphics
{
    public partial class QueryPool
    {
        internal int[] NativeQueries;

        public bool TryGetData(long[] dataArray)
        {
            for (var index = 0; index < NativeQueries.Length; index++)
            {
#if XENKO_GRAPHICS_API_OPENGLES
                GL.Ext.GetQueryObject(NativeQueries[index], GetQueryObjectParam.QueryResultAvailable, out long availability);
#else
                GL.GetQueryObject(NativeQueries[index], GetQueryObjectParam.QueryResultAvailable, out long availability);
#endif
                if (availability == 0)
                    return false;

#if XENKO_GRAPHICS_API_OPENGLES
                GL.Ext.GetQueryObject(NativeQueries[index], GetQueryObjectParam.QueryResult, out dataArray[index]);
#else
                GL.GetQueryObject(NativeQueries[index], GetQueryObjectParam.QueryResult, out dataArray[index]);
#endif
            }

            return true;
        }

        /// <inheritdoc/>
        protected internal override void OnDestroyed()
        {
            GL.DeleteQueries(QueryCount, NativeQueries);
            NativeQueries = null;
            base.OnDestroyed();
        }

        private void Recreate()
        {
            switch (QueryType)
            {
                case QueryType.Timestamp:
                    break;

                default:
                    throw new NotImplementedException();
            }

            NativeQueries = new int[QueryCount];
            GL.GenQueries(QueryCount, NativeQueries);
        }
    }
}

#endif
