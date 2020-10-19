﻿/****************************************************************************
 * Copyright (c) 2017 xiaojun
 * Copyright (c) 2017 imagicbell
 * Copyright (c) 2018.5 ~ 2020.5  liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System.Linq;

namespace QFramework
{
	//// <summary>
	/// <inheritdoc />
	/// <![CDATA[The 'member' start tag on line 2 position 2 does not match the end tag of 'summary'. Line 3, position 3.]]>
	[MonoSingletonPath("UIRoot/Manager")]
	public partial class UIManager : QMgrBehaviour, ISingleton
	{
		void ISingleton.OnSingletonInit()
		{

		}

		private static UIManager mInstance;

		public static UIManager Instance
		{
			get
			{
				if (!mInstance)
				{
					mInstance = MonoSingletonProperty<UIManager>.Instance;
				}

				return mInstance;
			}
		}

		public IPanel OpenUI(PanelSearchKeys panelSearchKeys)
		{
			var retPanel = UIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

			if (retPanel == null)
			{
				retPanel = CreateUI(panelSearchKeys);
			}

			retPanel.Open(panelSearchKeys.UIData);
			retPanel.Show();
			return retPanel;
		}

		/// <summary>
		/// 显示UIBehaiviour
		/// </summary>
		/// <param name="uiBehaviourName"></param>
		public void ShowUI(PanelSearchKeys panelSearchKeys)
		{
			var retPanel = UIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

			if (retPanel != null)
			{
				retPanel.Show();
			}
		}

		/// <summary>
		/// 隐藏UI
		/// </summary>
		/// <param name="uiBehaviourName"></param>
		public void HideUI(PanelSearchKeys panelSearchKeys)
		{
			var retPanel = UIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

			if (retPanel != null)
			{
				retPanel.Hide();
			}
		}

		/// <summary>
		/// 删除所有UI层
		/// </summary>
		public void CloseAllUI()
		{
			foreach (var layer in UIKit.Table)
			{
				layer.Close();
			}

			UIKit.Table.Clear();
		}

		/// <summary>
		/// 隐藏所有 UI
		/// </summary>
		public void HideAllUI()
		{
			UIKit.Table.ForEach(dataItem => dataItem.Hide());
		}

		/// <summary>
		/// 关闭并卸载UI
		/// </summary>
		/// <param name="behaviourName"></param>
		public void CloseUI(PanelSearchKeys panelSearchKeys)
		{
			var panel = UIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

			if (panel as UIPanel)
			{
				panel.Close();
				UIKit.Table.Remove(panel);
			}
		}

		public void RemoveUI(PanelSearchKeys panelSearchKeys)
		{
			var panel = UIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault();

			if (panel != null)
			{
				UIKit.Table.Remove(panel);
			}
		}

		/// <summary>
		/// 获取UIBehaviour
		/// </summary>
		/// <param name="uiBehaviourName"></param>
		/// <returns></returns>
		public UIPanel GetUI(PanelSearchKeys panelSearchKeys)
		{
			return UIKit.Table.GetPanelsByPanelSearchKeys(panelSearchKeys).FirstOrDefault() as UIPanel;
		}

		public override int ManagerId
		{
			get { return QMgrID.UI; }
		}

		public IPanel CreateUI(PanelSearchKeys panelSearchKeys)
		{
			var panel = UIKit.Config.LoadPanel(panelSearchKeys);

			UIKit.Root.SetLevelOfPanel(panelSearchKeys.Level, panel);

			UIKit.Config.SetDefaultSizeOfPanel(panel);

			panel.Transform.gameObject.name = panelSearchKeys.GameObjName ?? panelSearchKeys.PanelType.Name;

			panel.Info = new PanelInfo()
			{
				AssetBundleName = panelSearchKeys.AssetBundleName,
				Level = panelSearchKeys.Level,
				GameObjName = panelSearchKeys.GameObjName,
				PanelType = panelSearchKeys.PanelType,
				UIData = panelSearchKeys.UIData
			};

			UIKit.Table.Add(panel);

			panel.Init(panelSearchKeys.UIData);

			return panel;
		}
	}
}