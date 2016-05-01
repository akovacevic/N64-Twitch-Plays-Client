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

        private void IrcClient_LocalUser_LeftChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            e.Channel.UserJoined -= IrcClient_Channel_UserJoined;
            e.Channel.UserLeft -= IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived -= IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived -= IrcClient_Channel_NoticeReceived;

            Console.WriteLine("You left the channel {0}.", e.Channel.Name);
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                info.Content = String.Format("You left the channel {0}", e.Channel.Name);
            });
        }

        private void IrcClient_LocalUser_JoinedChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;


            e.Channel.UserJoined += IrcClient_Channel_UserJoined;
            e.Channel.UserLeft += IrcClient_Channel_UserLeft;
            e.Channel.MessageReceived += IrcClient_Channel_MessageReceived;
            e.Channel.NoticeReceived += IrcClient_Channel_NoticeReceived;

            Console.WriteLine("You joined the channel {0}.", e.Channel.Name);
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

            Console.WriteLine("[{0}] Notice: {1}.", channel.Name, e.Text);

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
                    //_botSender.SendMessage(_superlaggkingChannel,"A");
                    _writeToConsole(e.Source.Name, "A");

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0,new Input() { Username = e.Source.Name, ButtonImage = Images.A});
                    });
                }
                //b
                if (e.Text.IndexOf("yea", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("b", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('c');
                    //_botSender.SendMessage(_superlaggkingChannel, "B");
                    _writeToConsole(e.Source.Name, "B");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0,new Input() { Username = e.Source.Name, ButtonImage = Images.B });
                    });
                }
                //dl
                if (e.Text.IndexOf("ayy", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("dl", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('4');
                    //_botSender.SendMessage(_superlaggkingChannel, "D-pad Left");
                    _writeToConsole(e.Source.Name, "D-pad Left");

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dl });
                    });
                }
                //dr
                if (e.Text.IndexOf("nice", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("dr", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('6');
                    //_botSender.SendMessage(_superlaggkingChannel, "D-pad Right" );
                    _writeToConsole(e.Source.Name, "D-pad Right");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dr });
                    });
                }
                //du
                if (e.Text.IndexOf("rip", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("du", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('8');
                    //_botSender.SendMessage(_superlaggkingChannel, "D-pad Up");
                    _writeToConsole(e.Source.Name, "D-pad Up");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Du });
                    });
                }
                //dd
                if (e.Text.IndexOf("ohh", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("dd", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('2');
                    //_botSender.SendMessage(_superlaggkingChannel, "D-pad Down");
                    _writeToConsole(e.Source.Name, "D-pad Down");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dd });
                    });
                }
                //cl
                if (e.Text.IndexOf("woo", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cl", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('j');
                    //_botSender.SendMessage(_superlaggkingChannel, "D-pad Left");
                    _writeToConsole(e.Source.Name, "C-Left");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cl });
                    });
                }
                //cr
                if (e.Text.IndexOf("hi", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cr", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('l');
                    //_botSender.SendMessage(_superlaggkingChannel, "D-pad Right");
                    _writeToConsole(e.Source.Name, "C-Right");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cr });
                    });
                }
                //cu
                if (e.Text.IndexOf("gg", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cu", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('i');
                    //_botSender.SendMessage(_superlaggkingChannel, "C- Up");
                    _writeToConsole(e.Source.Name, "C-UP");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cu });
                    });
                }
                //cd
                if (e.Text.IndexOf("wtf", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("cd", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('k');
                    //_botSender.SendMessage(_superlaggkingChannel, "C Down");
                    _writeToConsole(e.Source.Name, "C-Down");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Cd });
                    });
                }
                //z
                if (e.Text.IndexOf("ily", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("z", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('z');
                    //_botSender.SendMessage(_superlaggkingChannel, "Z");
                    _writeToConsole(e.Source.Name, "Z");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Z });
                    });
                }
                //al
                if (e.Text.IndexOf("omg", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("al", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)37);
                    //_botSender.SendMessage(_superlaggkingChannel, "Analog Left");
                    _writeToConsole(e.Source.Name, "Analog Left");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dl });
                    });
                }
                //ar
                if (e.Text.IndexOf("savage", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("ar", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)39);
                    //_botSender.SendMessage(_superlaggkingChannel, "Analog Right");
                    _writeToConsole(e.Source.Name, "Analog Right");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dr });
                    });
                }
                //au
                if (e.Text.IndexOf("lmao", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("au", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)38);
                    //_botSender.SendMessage(_superlaggkingChannel, "Analog Up");
                    _writeToConsole(e.Source.Name, "Analog Up");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Du });
                    });
                }
                //ad
                if (e.Text.IndexOf("lul", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("ad", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)40);
                    //_botSender.SendMessage(_superlaggkingChannel, "analog Down");
                    _writeToConsole(e.Source.Name, "Analog Down");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Dd });
                    });
                }
                //start
                if (e.Text.IndexOf("wow", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("start", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar((char)13);
                    //_botSender.SendMessage(_superlaggkingChannel, "Start");
                    _writeToConsole(e.Source.Name, "Start");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.Start });
                    });
                }
                //l
                if (e.Text.IndexOf("omfg", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("l", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('a');
                    //_botSender.SendMessage(_superlaggkingChannel, "L");
                    _writeToConsole(e.Source.Name, "L");
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0, new Input() { Username = e.Source.Name, ButtonImage = Images.L });
                    });
                }
                //r
                if (e.Text.IndexOf("fag", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("r", StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('s');
                    //_botSender.SendMessage(_superlaggkingChannel, "R");
                    _writeToConsole(e.Source.Name, "R");
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

        private void IrcClient_Channel_UserLeft(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;
            Console.WriteLine("[{0}] User {1} left the channel.", channel.Name, e.ChannelUser.User.NickName);
        }

        private void IrcClient_Channel_UserJoined(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;
            Console.WriteLine("[{0}] User {1} joined the channel.", channel.Name, e.ChannelUser.User.NickName);
        }

        private void IrcClient_LocalUser_MessageReceived(object sender, IrcMessageEventArgs e)
        {
            if (e.Source is IrcUser)
            {
                // Read message.
                Console.WriteLine("({0}): {1}.", e.Source.Name, e.Text);
            }
            else
            {
                Console.WriteLine("({0}) Message: {1}.", e.Source.Name, e.Text);
            }
        }

        private void IrcClient_LocalUser_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;
            Console.WriteLine("Notice: {0}.", e.Text);
        }

        private static void IrcClient_Disconnected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;
        }

        private static void IrcClient_Connected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;
        }

        private static void _writeToConsole(string name, string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("[" + name + "]");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" : " + text);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

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
                            Console.WriteLine("Connection to '{0}' timed out.", server);
                            return;
                        }
                    }
                    Console.Out.WriteLine("Now connected to '{0}'.", server);
                    if (!registeredEvent.Wait(10000))
                    {
                        Console.WriteLine("Could not register to '{0}'.", server);
                        return;
                    }
                }

                Console.Out.WriteLine("Now registered to '{0}' as '{1}'.", server, username);
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
