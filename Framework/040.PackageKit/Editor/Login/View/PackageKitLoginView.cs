using System.ComponentModel;
using QFramework.PackageKit.State;

namespace QFramework.PackageKit
{
    [DisplayName("账户")]
    [PackageKitRenderOrder(int.MaxValue)]
    public class PackageKitLoginView : VerticalLayout, IPackageKitView
    {
        public IQFrameworkContainer Container { get; set; }

        ControllerNode<PackageKitLoginApp> mControllerNode = ControllerNode<PackageKitLoginApp>.Allocate();

        public bool Ignore { get; private set; }

        public bool Enabled
        {
            get { return true; }
        }

        DisposableList mDisposableList = new DisposableList();

        public void Init(IQFrameworkContainer container)
        {
            EasyIMGUI.Label().Text("账户信息").FontSize(12).AddTo(this);

            var boxLayout = new VerticalLayout("box").AddTo(this);
            
            var logoutBtn = EasyIMGUI.Button().Text("注销")
                .Visible(PackageKitLoginState.Logined.Value)
                .AddTo(boxLayout);

            var loginView = new LoginView()
                .Do(self => self.Visible = PackageKitLoginState.LoginViewVisible.Value)
                .AddTo(boxLayout);

            var registerView = new RegisterView()
                .Do(self => self.Visible = PackageKitLoginState.RegisterViewVisible.Value)
                .AddTo(boxLayout);

            PackageKitLoginState.Logined.Bind(value => { logoutBtn.Visible = value; }).AddTo(mDisposableList);

            
            logoutBtn.OnClick(mControllerNode.SendCommand<LogoutCommand>);

            PackageKitLoginState.LoginViewVisible.Bind(value => { loginView.Visible = value; }).AddTo(mDisposableList);


            PackageKitLoginState.RegisterViewVisible.Bind(value => { registerView.Visible = value; })
                .AddTo(mDisposableList);
        }

        public void OnUpdate()
        {
        }

        void IPackageKitView.OnGUI()
        {
            DrawGUI();
        }


        public class LocaleText
        {
            public static string UserInfo
            {
                get { return Language.IsChinese ? "用户信息" : "User Info"; }
            }
        }

        public void OnDispose()
        {
            mControllerNode.Recycle2Cache();
            mControllerNode = null;

            mDisposableList.Dispose();
            mDisposableList = null;
        }
    }
}