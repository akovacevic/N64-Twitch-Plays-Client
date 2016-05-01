# N64-Twitch-Plays-Client
An N64 Twitch plays client, that takes input from twitch chats, feeds it as a keyboard input, and displays the keys being pressed. In C# WPF

### In the app.config:

    <appSettings>
        <add key="Username" value=""/>
        <add key="Password" value=""/>
        <add key="Server" value ="irc.twitch.tv"/>
    </appSettings>
    
Add your Twitch username and an oauth password, which you can get from https://twitchapps.com/tmi/

### Run the program
+ Hit Connect button at the bottom left.
+ Type a Twitch Chat and click Join
+ Make sure your game is the active window
+ You can pause and continue the keyboard button presses with the Pause button

All Done, Enjoy!

![Alt text](/screenCapture.png?raw=true "Optional Title")

## Configure your own keys:
In the MainWINdow.xaml.cs page

Go to the IrcClient_channel_MessageReceived method:
+ Change the if statement to change the Twitch chat input
+ edit the InputReader.SendInputChar to change the Keyboard button that is pressed


        if (e.Text.IndexOf("lol", StringComparison.InvariantCultureIgnoreCase) >= 0 || e.Text.Equals("a",StringComparison.InvariantCultureIgnoreCase))
                {
                    InputReader.SendInputChar('x');
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _inputs.Insert(0,new Input() { Username = e.Source.Name, ButtonImage = Images.A});
                    });
                }
                
## Adding More images
Add your image in the images folder, making sure to set it as "Conent" and "Copy always" in the properties settings

Add a new public method for it:

        public static string A { get; private set; }
        
in the static constructor, set it to the image path:

     static Images()
        {
            A = _getFullPath("a");
        }

This will set the image "a.png" in the images folder to the public string that can be accessed when you call Images.A

