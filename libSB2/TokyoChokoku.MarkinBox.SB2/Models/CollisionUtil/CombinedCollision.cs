using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.CollisionUtil
{
    public class CombinedCollision : ICollision
    {
        private readonly Entry[] entries;

        public IEnumerable<(string Key, ICollision Collision)> Collisions =>
            from e in entries
            select (e.Key, e.Collision);


        /// <summary>
        /// このクラスのビルダー <c>CombinedCollision.Builder</c> を作成します．
        /// </summary>
        /// <returns>The builder.</returns>
        public static Builder CreateBuilder () {
            return new Builder ();
        }



        private CombinedCollision (Entry[] entries)
        {
            this.entries = entries;
        }



        public R Accept<T, R>(ICollisionVisitor<T, R> visitor, T args)
        {
            return visitor.Visit(this, args);
        }


        /// <summary>
        /// 指定された点と 衝突判定を行います．
        /// このインスタンスが持つ衝突判定の中で，最初に成功したものについて返り値が設定されます．
        /// 具体的には，<c>CombinedCollision.Builder.Add</c>の Addメソッドで指定した 文字列キーが設定されます．
        /// </summary>
        /// <param name="point">point.</param>
        /// <returns> 
        /// CombinedCollision.Resultインスタンス．
        /// 
        /// CombinedCollisionインスタンスが持つ衝突判定の中でいずれか1つ成功した場合，
        /// <c>Result.IsCollided</c>は trueを返します．そうでなければ falseを返します．
        /// 
        /// <c>Result.Key</c>には 最初に成功した衝突判定に設定された文字列キーを返します．
        /// これは <c>CombinedCollision.Builder.Add</c>で設定された文字列キーのことです．
        /// </returns>
        public Result At (Homogeneous2D point) {
            foreach (var entry in entries) {
                if (entry.Collision.At (point)) {
                    return new Result (
                        key : entry.Key
                    );
                }
            }

            return new Result ();
        }


        /// <summary>
        /// 指定された円と 衝突判定を行います．
        /// このインスタンスが持つ衝突判定の中で，最初に成功したものについて返り値が設定されます．
        /// 具体的には，<c>CombinedCollision.Builder.Add</c>の Addメソッドで指定した 文字列キーが設定されます．
        /// </summary>
        /// <param name="point">point.</param>
        /// <returns> 
        /// CombinedCollision.Resultインスタンス．
        /// 
        /// CombinedCollisionインスタンスが持つ衝突判定の中でいずれか1つ成功した場合，
        /// <c>Result.IsCollided</c>は trueを返します．そうでなければ falseを返します．
        /// 
        /// <c>Result.Key</c>には 最初に成功した衝突判定に設定された文字列キーを返します．
        /// これは <c>CombinedCollision.Builder.Add</c>で設定された文字列キーのことです．
        /// </returns>
        public Result OnCircle (Homogeneous2D origin, double radius) {

            foreach (var entry in entries) {
                if (entry.Collision.OnCircle (origin, radius)) {
                    return new Result (
                        key : entry.Key
                    );
                }
            }

            return new Result ();
        }



        bool ICollision.At (Homogeneous2D point) {
            foreach (var entry in entries) {
                if (entry.Collision.At (point)) {
                    return true;
                }
            }

            return false;
        }



        bool ICollision.OnCircle (Homogeneous2D origin, double radius)
        {
            foreach (var entry in entries) {
                if (entry.Collision.OnCircle (origin, radius)) {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 箱との衝突判定
        /// </summary>
        /// <returns><c>true</c>, if box was oned, <c>false</c> otherwise.</returns>
        bool ICollision.InBox (RectangleArea rect)
        {
            foreach (var entry in entries) {
                if (!entry.Collision.InBox (rect))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// CombinedCollision での衝突判定の結果を表現します．
        /// </summary>
        public sealed class Result {


            /// <summary>
            /// CombinedCollisionの衝突判定に成功したかどうか取得します．
            /// </summary>
            /// <value>CombinedCollisionのインスタンスに渡された衝突判定が1つでも成功したなら <c>true</c>．
            /// そうでなければ <c>false</c>.</value>
            public bool   IsCollided { get; }


            /// <summary>
            /// 一番はじめに成功した衝突判定の 文字列キーを取得します．
            /// これは  <c>CombinedCollision.Builder.Add</c>で設定された文字列キーのことです．
            /// </summary>
            /// <value>文字列キー</value>
            public string Key        { get; }



            internal Result (string key) {
                IsCollided = true;
                Key = key;
            }

            internal Result () {
                IsCollided = false;
                Key = "";
            }
        }



        /// <summary>
        /// CombinedCollision のビルダーです．
        /// </summary>
        public sealed class Builder {
            private readonly List<Entry> entries = new List<Entry> ();

            internal Builder () {}


            /// <summary>
            /// 衝突判定とそれを表す文字列キーを指定して，衝突判定を追加します．
            /// </summary>
            /// 
            /// <exception cref="NullReferenceException">
            /// key もしくは collision がnullであった時．
            /// </exception>
            public void AddCollision (string key, ICollision collision) {
                entries.Add (new Entry (key, collision));
            }


            /// <summary>
            /// CombinedCollision のインスタンスを作成します．
            /// </summary>
            public CombinedCollision Build () {
                return new CombinedCollision (entries.ToArray ());
            }
        }



        private sealed class Entry {
            public ICollision Collision { get; }
            public string     Key       { get; }

            internal Entry (string key, ICollision collision) {
                if (key == null || collision == null)
                    throw new NullReferenceException ();

                Collision = collision;
                Key = key;
            }
        }


    }
}

