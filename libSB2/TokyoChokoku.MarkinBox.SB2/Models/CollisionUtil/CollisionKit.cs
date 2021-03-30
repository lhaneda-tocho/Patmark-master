using System;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    public class CollisionKit2 <Type> where Type:class
    {
        private readonly CollisionKitDataSource<Type> source;
        private Type lastCollision = null;

        public Type LastCollision { 
            get { return lastCollision; }
            set { lastCollision = value; }
        }


        public CollisionKit2 (CollisionKitDataSource<Type> source)
        {
            this.source = source;
        }


        public bool Contains(Type value)
        {
            return source.DataList ().Contains (value);
        }


        public ICollision GetCollision(Type value)
        {
            return source.GetCollision (value);
        }


        /// <summary>
        /// タッチ位置との衝突判定を行います．
        /// </summary>
        /// <returns>
        /// MBObjectとの衝突判定に 成功した場合，そのMBObjectを 返します．
        /// 何も衝突していない場合は nullを返します．
        /// </returns>
        /// <param name="point">タッチ位置を表すベクトル</param>
        public Type At(Homogeneous2D point)
        {
            Type collided = null;
            bool afterLastCollision = (lastCollision==null);

            // 前回の衝突判定の後から スタートし，
            // 一番最初に成功した衝突判定を collidedに代入します．
            // 衝突判定に失敗した場合， collidedはnullのままです．

            // 実際は dictionaryの一番最初の要素からのスタートになるので，
            // 上記の処理と同じになるように工夫します．
            foreach( var value in source.DataList() )
            {
                var collision = source.GetCollision (value);

                if (!afterLastCollision) {
                    afterLastCollision |= (lastCollision == value);

                    // ここで 暫定的な判定を行う．初めに判定を成功するまで行う．
                    if (collided == null && collision.At (point)) {
                        // 衝突判定に成功した場合．
                        collided = value;
                    }
                } else {
                    // 前回の衝突判定より後の要素で判定を行う．
                    if (collision.At (point)) {
                        // 衝突判定に成功した場合．
                        collided = value;
                        break;
                    }
                }
            }

            lastCollision = collided ?? lastCollision;
            return collided;
        }


        public Type OnCircle(Homogeneous2D origin, double radius)
        {
            Type collided = null;
            bool afterLastCollision = (lastCollision==null);

            // 前回の衝突判定の後から スタートし，
            // 一番最初に成功した衝突判定を collidedに代入します．
            // 衝突判定に失敗した場合， collidedはnullのままです．

            // 実際は dictionaryの一番最初の要素からのスタートになるので，
            // 上記の処理と同じになるように工夫します．
            foreach( var value in source.DataList() )
            {
                var collision = source.GetCollision (value);

                if (!afterLastCollision) {
                    afterLastCollision |= (lastCollision == value);

                    // ここで 暫定的な判定を行う．初めに判定を成功するまで行う．
                    if (collided == null && collision.OnCircle (origin, radius)) {
                        // 衝突判定に成功した場合．
                        collided = value;
                    }
                } else {
                    // 前回の衝突判定より後の要素で判定を行う．
                    if (collision.OnCircle (origin, radius)) {
                        // 衝突判定に成功した場合．
                        collided = value;
                        break;
                    }
                }
            }

            lastCollision = collided ?? lastCollision;
            return collided;
        }

        public Type OnCircle (Homogeneous2D origin, double radius, HashSet<Type> ignores)
        {
            Type collided = null;
            bool afterLastCollision = (lastCollision == null);

            // 前回の衝突判定の後から スタートし，
            // 一番最初に成功した衝突判定を collidedに代入します．
            // 衝突判定に失敗した場合， collidedはnullのままです．

            // 実際は dictionaryの一番最初の要素からのスタートになるので，
            // 上記の処理と同じになるように工夫します．
            foreach (var value in source.DataList ()) {
                var collision = source.GetCollision (value);

                if (!afterLastCollision) {
                    // afeter last collision の更新
                    afterLastCollision |= (lastCollision == value);

                    // 無視するオブジェクトの時は何もしない．
                    if (ignores.Contains (value))
                        continue;
                        
                    // ここで 暫定的な判定を行う．初めに判定を成功するまで行う．
                    if (collided == null && collision.OnCircle (origin, radius)) {
                        // 衝突判定に成功した場合．
                        collided = value;
                    }
                } else {
                    // 無視するオブジェクトの時は何もしない．
                    if (ignores.Contains (value))
                        continue;
                    
                    // 前回の衝突判定より後の要素で判定を行う．
                    if (collision.OnCircle (origin, radius)) {
                        // 衝突判定に成功した場合．
                        collided = value;
                        break;
                    }
                }
            }

            lastCollision = collided ?? lastCollision;
            return collided;
        }


    }
}

