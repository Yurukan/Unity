using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// テキストの文字を逐次的に表示するクラス。表示中の文字を半透明にする
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class SeveralCharsDisplayer : UIBehaviour, IMeshModifier, ITextShower
    {
        [SerializeField]
        float   _secPerChar = 0.1f;

        [SerializeField]
        int     _alpLength = 3;

        Text    _text;
        float[] _alpValues;
        int     _curIndex;
        bool    _isShowing;
        float   _time;

        /// <summary>
        /// ITextShowerを参照
        /// </summary>
        public event Action TextShowed;

		/// <summary>
		/// インスタンス生成直後に実行される処理
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			_text = GetComponent<Text>();
			_curIndex = -1;
			_isShowing = false;
			_time = 0f;
			TextShowed = () => { };

			// アルファ値の変化量
			float delta = 1f / (_alpLength + 1);

			// アルファ値の深度
			_alpValues = new float[_alpLength];
			for (int i = 0; i < _alpLength; i++)
				_alpValues[i] = (_alpLength - i) * delta;

			// 初期状態を反映
			_text.SetVerticesDirty();
		}

        /// <summary>
        /// ITextShowerを参照
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            _curIndex = -1;
            _text.text = (text != null) ? text : string.Empty;
        }

        /// <summary>
        /// ITextShowerを参照
        /// </summary>
        public void ShowText()
        {
			if (_curIndex < _text.text.Length + _alpLength)
			{
				// 表示中の場合、全ての文字を表示
				if (_isShowing)
				{
					ShowAll();
				}
				// 未表示の場合、表示開始
				else
				{
					StartCoroutine(ShowCharacters());
					_isShowing = true;
				}
			}
        }

		/// <summary>
		/// テキスト内の文字を逐次表示する
		/// </summary>
		/// <returns></returns>
		IEnumerator ShowCharacters()
		{
			while (_curIndex < _text.text.Length + _alpLength)
			{
				// 次の文字を表示
				if (_time <= 0f)
				{
					ShowNextCharacter();
					_time = _secPerChar;
				}
				else
				{
					_time -= Time.deltaTime;
					yield return 0;
				}
			}

			_isShowing = false;
			TextShowed();
		}

		/// <summary>
		/// 次の文字を表示
		/// </summary>
		void ShowNextCharacter()
		{
			_curIndex++;

			// 頂点が変更されることを通知
			_text.SetVerticesDirty();
		}

		/// <summary>
		/// 全ての文字を表示
		/// </summary>
		void ShowAll()
		{
			_curIndex = _text.text.Length + _alpLength;

			// 頂点が変更されることを通知
			_text.SetVerticesDirty();
		}

		/// <summary>
		/// メッシュを変形する
		/// </summary>
		/// <param name="verts">メッシュ編集のヘルパークラス</param>
        void IMeshModifier.ModifyMesh(VertexHelper verts)
        {
			if (_text == null)
				return;

            // 頂点データをコピー
			List<UIVertex> stream = ListPool<UIVertex>.Get();
			verts.GetUIVertexStream(stream);

			// アルファ値の適用
			if (_alpLength > 0)
				SetAlphaToVertexes(stream, _alpValues, 
					_curIndex - _alpLength + 1, _curIndex + 1);

			// 非表示
			SetAlphaInVertexes(stream, 0f, _curIndex + 1);

            // 新しい頂点データを適用
			verts.Clear();
			verts.AddUIVertexTriangleStream(stream);

			ListPool<UIVertex>.Release(stream);
        }

        void IMeshModifier.ModifyMesh(Mesh mesh)
        {
            // 何もしない
        }

        /// <summary>
        /// 範囲内の頂点データのアルファ値を設定
        /// </summary>
        /// <param name="stream">頂点データ</param>
        /// <param name="alphas">アルファ値の配列</param>
        /// <param name="begin">範囲の開始インデックス</param>
        /// <param name="end">範囲の終了インデックス。境界は含まない</param>
		void SetAlphaToVertexes(List<UIVertex> stream, float[] alphas, int begin, int end)
        {
            Color    color;
            UIVertex element;

            // アルファ値の開始インデックス
            int alpBegin = _alpLength - _curIndex - 1;
            alpBegin = (alpBegin > 0) ? alpBegin : 0;

            // 頂点インデックスに変換
            begin = (begin > 0) ? 6*begin : 0;
            end = (end < _text.text.Length) ? 6*end : stream.Count;

            // アルファ値を適用
            for (int i = begin, j = alpBegin; i < end; i += 6, j++)
            {
                for (int r = 0; r < 6; r++)
                {
                    element = stream[i + r];

                    color = element.color;
                    color.a *= alphas[j];
                    element.color = color;

                    stream[i + r] = element;
                }
            }
        }

		void SetAlphaInVertexes(List<UIVertex> stream, float alpha, int begin)
        {
            Color    color;
            UIVertex element;

            // 頂点インデックスに変換
            begin = (begin > 0) ? 6*begin : 0;

            // アルファ値を適用
            for (int i = begin; i < stream.Count; i++)
            {
                element = stream[i];

                color = element.color;
                color.a = alpha;
                element.color = color;

                stream[i] = element;
            }
        }
    }
}