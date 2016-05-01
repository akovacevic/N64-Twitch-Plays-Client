using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IrcDotNet;

namespace MainIrcFeed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool pushKeys = true;
        private static ObservableCollection<Input> _inputs;
        private IrcDotNet.TwitchIrcClient client;
        private static bool _notify = false;


        public MainWindow()
        {
            InitializeComponent();

            var iconUri = new Uri(Images.A);

            this.Icon = BitmapFrame.Create(iconUri);

            _inputs = new ObservableCollection<Input>();
            listView.ItemsSource = _inputs;
            listView.Background = Brushes.Transparent;
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var channelName = this.channelName.Text;

            if (client != null)
            {
                client.SendRawMessage(@"join #" + channelName);
                this.channelName.Text = "";
            }
            else
            {
                this.info.Content = "Not Connected: Connect, then join";
            }
            
        }

        private void IrcClient_Registered(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            client.LocalUser.NoticeReceived += IrcClient_LocalUser_NoticeReceived;
            client.LocalUser.MessageReceived += IrcClient_LocalUser_MessageReceived;
            client.LocalUser.JoinedChannel += IrcClient_LocalUser_JoinedChannel;
            client.LocalUser.LeftChannel += IrcClient_LocalUser_LeftChannel;
        }

        private void IrcClient_LocalUser_JoinedChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;


            e.Channel.UserJoined += IrcClient_Channel_UserJoined;
            e.Channel.UserLeft += IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived += IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived += IrcClient_Channel_NoticeReceived;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                info.Content = String.Format("You Joined the channel {0}", e.Channel.Name);
            });

            if(_notify)
                localUser.SendMessage(e.Channel, "This channel will be used to feed game input for my game");
        }

        private void IrcClient_Channel_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var channel = (IrcChannel)sender;


            App.Current.Dispatcher.Invoke((Action)delegate
            {
                info.Content = String.Format("[{0}] Notice: {1}.", channel.Name, e.Text);
            });
        }

        private void IrcClient_Channel_MessageReceived(object sender, IrcMessageEventArgs e)
        {
            if (pushKeys)
            {
                //a
                if (e.Text.IndexOf("lol", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("a",StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('x');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0,new Input() { Username = e.Source.Name, ButtonImage = Images.A});
                    });
                }
                //b
                if (e.Text.IndexOf("yea", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("b", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('c');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0,new Input() { Username = e.Source.Name, ButtonImage = Images.B });
                    });
                }
                //dl
                if (e.Text.IndexOf("ayy", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("dl", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('4');

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dl });
                    });
                }
                //dr
                if (e.Text.IndexOf("nice", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("dr", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('6');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dr });
                    });
                }
                //du
                if (e.Text.IndexOf("rip", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("du", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('8');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Du });
                    });
                }
                //dd
                if (e.Text.IndexOf("ohh", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("dd", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('2');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dd });
                    });
                }
                //cl
                if (e.Text.IndexOf("woo", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cl", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('j');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cl });
                    });
                }
                //cr
                if (e.Text.IndexOf("hi", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cr", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('l');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cr });
                    });
                }
                //cu
                if (e.Text.IndexOf("gg", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cu", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('i');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cu });
                    });
                }
                //cd
                if (e.Text.IndexOf("wtf", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cd", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('k');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cd });
                    });
                }
                //z
                if (e.Text.IndexOf("ily", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("z", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('z');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Z });
                    });
                }
                //al
                if (e.Text.IndexOf("omg", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("al", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)37);
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dl });
                    });
                }
                //ar
                if (e.Text.IndexOf("savage", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("ar", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)39);
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dr });
                    });
                }
                //au
                if (e.Text.IndexOf("lmao", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("au", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)38);
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Du });
                    });
                }
                //ad
                if (e.Text.IndexOf("lul", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("ad", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)40);
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dd });
                    });
                }
                //start
                if (e.Text.IndexOf("wow", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("start", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)13);
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Start });
                    });
                }
                //l
                if (e.Text.IndexOf("omfg", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("l", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('a');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.L });
                    });
                }
                //r
                if (e.Text.IndexOf("fag", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("r", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('s');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.R });
                    });
                }
            }

            if (_inputs.Count > 25)
            {
                int i = _inputs.Count - 1;
                while (i > 20)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.RemoveAt(i);
                    });
                    i--;
                }
            }

            if (e.Source is IrcUser)
            {
                // Read message.
                //Console.WriteLine("[{0}]({1}): {2}", channel.Name, e.Source.Name, e.Text);
            }
            else
            {
                //Console.WriteLine("[{0}]({1}) Message: {2}", channel.Name, e.Source.Name, e.Text);
            }
        }
        #region unused

        private void IrcClient_LocalUser_LeftChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            e.Channel.UserJoined -= IrcClient_Channel_UserJoined;
            e.Channel.UserLeft -= IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived -= IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived -= IrcClient_Channel_NoticeReceived;

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                info.Content = String.Format("You left the channel {0}", e.Channel.Name);
            });
        }

        private void IrcClient_Channel_UserLeft(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;
            //Console.WriteLine("[{0}] User {1} left the channel.", channel.Name, e.ChannelUser.User.NickName);
        }

        private void IrcClient_Channel_UserJoined(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;
            //Console.WriteLine("[{0}] User {1} joined the channel.", channel.Name, e.ChannelUser.User.NickName);
        }

        private void IrcClient_LocalUser_MessageReceived(object sender, IrcMessageEventArgs e)
        {
            if (e.Source is IrcUser)
            {
                // Read message.
                //Console.WriteLine("({0}): {1}.", e.Source.Name, e.Text);
            }
            else
            {
                //Console.WriteLine("({0}) Message: {1}.", e.Source.Name, e.Text);
            }
        }

        private void IrcClient_LocalUser_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;
            //Console.WriteLine("Notice: {0}.", e.Text);
        }

        private static void IrcClient_Disconnected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;
        }

        private static void IrcClient_Connected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
#endregion

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            if (pushKeys)
            {
                pushKeys = false;
                this.pause.Content = "Continue";
            }
            else
            {
                pushKeys = true;
                this.pause.Content = "Pause";
            }
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            var server = ConfigurationManager.AppSettings.Get("Server");
            var username = ConfigurationManager.AppSettings.Get("Username");
            var password = ConfigurationManager.AppSettings.Get("Password");

            if (client == null)
            {

                client = new IrcDotNet.TwitchIrcClient();
                client.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
                client.Disconnected += IrcClient_Disconnected;
                client.Registered += IrcClient_Registered;

                using (var registeredEvent = new ManualResetEventSlim(false))
                {
                    using (var connectedEvent = new ManualResetEventSlim(false))
                    {
                        client.Connected += (sender2, e2) => connectedEvent.Set();
                        client.Registered += (sender2, e2) => registeredEvent.Set();
                        client.Connect(server, false,
                            new IrcUserRegistrationInfo()
                            {
                                NickName = username,
                                Password = password,
                                UserName = username
                            });
                        if (!connectedEvent.Wait(10000))
                        {
                            info.Content = string.Format("Connection to '{0}' timed out", server);
                            return;
                        }
                    }

                    info.Content = string.Format("Now connected to '{0}'", server);
                    if (!registeredEvent.Wait(10000))
                    {
                        info.Content = string.Format("Could not register to '{0}'", server);
                        return;
                    }
                }

                info.Content = string.Format("Now Registered to {0} as {1}", server, username);
                disconnect.Content = "Disconnect";
            }
            else
            {
                client.Disconnect();
                client = null;
                disconnect.Content = "Connect";
                info.Content = string.Format("Disconnected");
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            _notify = true;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _notify = false;
        }
    }

}
