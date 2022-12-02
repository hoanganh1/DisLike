using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using File = System.IO.File;

namespace GPM_View
{
    public partial class Form1 : Form
    {
        List<string> listComments;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        List<account> lstAccount;
        List<string> lstProxy;
        int proxyNumber = 0;
        int numberRow = 0;
        Random random = new Random();
        private void btnEmail_Click(object sender, EventArgs e)
        {
            checkopentab = true;
            lstAccount = new List<account>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (ofd.FileName.Length > 0)
            {
                var lines = File.ReadAllLines(ofd.FileName);
                int num = 0;
                foreach (var line in lines)
                {
                    account account = new account();
                    account.stt = num; num += 1;
                    account.email = line.Split('\t')[0].Trim();
                    account.password = line.Split('\t')[1].Trim();
                    account.mail_kp = line.Split('\t')[2].Trim();
                    account.status = "";
                    lstAccount.Add(account);
                }
            }
            dataGrid.DataSource = lstAccount;
            dataGrid.Columns[0].Width = 50;
            dataGrid.Columns[1].Width = 100;
            dataGrid.Columns[2].Width = 70;
            dataGrid.Columns[3].Width = 50;
            dataGrid.Columns["status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            numberRow = 0;
        }
        void loadProxy()
        {
            lstProxy = new List<string>();
            var item = File.ReadAllLines("data\\proxy.txt");
            foreach (string line in item)
            {
                lstProxy.Add(line.Trim());
            }
        }
        void saveError(account mails, string error)
        {
            for (int i = 0; i < 1000; i++)
            {
                try { File.AppendAllText("Error.txt", mails.email + "|" + mails.password + "|" + mails.mail_kp + "|" + error + "\r\n"); break; } catch { Thread.Sleep(100); }
            }
        }
        void resave(string proxy)
        {
            lstProxy.Remove(proxy);
            try
            {
                File.WriteAllText("data\\proxy.txt", String.Join("\r\n", lstProxy));
            }
            catch { }
        }
        void createThread(int indexKichBan)
        {
            for (int i = 0; i < nbThread.Value; i++)
            {
                Thread st = new Thread(() =>
                {
                    run(indexKichBan);
                });
                st.Start();
                Thread.Sleep(1000);
            }
        }
        void addStatus(int index, string Text)
        {
            dataGrid.Rows[index].Cells["status"].Value = Text;
        }
        bool clickDeSau(UndetectChromeDriver driver)
        {
            try
            {
                driver.ExecuteScript("document.getElementsByClassName('VfPpkd-Jh9lGc')[0].click()"); return true;
            }
            catch
            {
                ClickXacMinh(driver);
                return false;
            }
        }
        bool ClickXacMinh(UndetectChromeDriver driver)
        {
            try
            {
              
                driver.ExecuteScript("document.getElementsByClassName('ZFr60d CeoRYc')[1].click()"); return true;
            }
            catch
            {
                return false;

            }
        }
        void addbirthday(UndetectChromeDriver driver)
        {
            driver.Navigate().GoToUrl(urlLogin);
            Thread.Sleep(7000);
        }
        List<string> list = new List<string>();

        string urlLogin = "https://accounts.google.com/signin/v2/identifier?service=lbc&passive=1209600&continue=https%3A%2F%2Fbusiness.google.com%2F%3FskipPagesList%3D1%26skipLandingPage%3Dtrue%26hl%3Den%26gmbsrc%3Dus-en-z-z-z-gmb-l-z-d~mhp-hom_sig-u&followup=https%3A%2F%2Fbusiness.google.com%2F%3FskipPagesList%3D1%26skipLandingPage%3Dtrue%26hl%3Den%26gmbsrc%3Dus-en-z-z-z-gmb-l-z-d~mhp-hom_sig-u&hl=en&flowName=GlifWebSignIn&flowEntry=ServiceLogin";

        List<JObject> profiles;
        void run(int indexKichBan)
        {   

            GPMLoginAPI api = new GPMLoginAPI("http://" + APP_URL.Text);
            profiles = api.GetProfiles();
            kichban1();
            
        }

        private void kichban1()
        {
            int index = numberRow;
            numberRow += 1;
            if (index >= dataGrid.Rows.Count)
            {
                return;
            }
            if (!checkopentab)
            {
                return;
            }
            account act = lstAccount[index];
            UndetectChromeDriver driver = null;
            try
            {
                /*addStatus(index, "starting");
                GPMLoginAPI api = new GPMLoginAPI("http://" + APP_URL.Text);
                acton sts = new acton(act, api);
                addStatus(index, "đang mở profile");
                Thread.Sleep(1000);
                JObject ojb = sts.getLst(act.email, profiles);
                string id_profile = "";
                if (ojb == null)
                {
                    int prox = proxyNumber;
                    loadProxy();
                    proxyNumber += 1;
                    if (prox >= lstProxy.Count)
                    {
                        proxyNumber = 0;
                        prox = proxyNumber;
                    }
                    string proxy = lstProxy[prox];
                    resave(proxy);

                    ojb = api.Create(act.email, proxy, true);
                    if (ojb != null)
                    {
                        //Tạo thành công
                        id_profile = ojb["profile_id"].ToString();
                        addStatus(index, "tạo profile thành công");
                        saveProfile(act, proxy);
                    }
                }
                else
                {
                    //đã có profile
                    id_profile = ojb["id"].ToString();
                }


                try { driver = sts.openProfile(id_profile, index); }
                catch
                {
                    addStatus(index, "Lỗi mở profile");
                    saveError(act, "Lỗi mở profile");
                    driver.Close();
                    driver.Dispose();
                    driver.Quit();
                    goto ketthuc;
                }*/

                int z_index = index % 300;
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                ChromeOptions options = new ChromeOptions();
                options.AddExcludedArgument("enable-automation");
                options.AddArguments("disable-infobars");
              

                driver = new UndetectChromeDriver(service, options);
                driver.Manage().Window.Position = new Point(50 * Convert.ToInt32(z_index / 25), 35 * Convert.ToInt32(z_index % 25));
                driver.Manage().Window.Size = new Size(1200, 800);

                try { driver.Get(urlLogin); } catch { }
                Thread.Sleep(2000);
                addStatus(index, "truy cập google");
                if (driver.Url.Contains("business.google.com/create/new"))
                {
                    goto searchz;
                }
                login st = new login(driver, act); int demnha = 0;
                string Error = string.Empty;
            lainha:
                if (st.Nanial(urlLogin))
                {
                    if (!st.StartLogin(out Error))
                    {
                        if (Error == "captcha")
                        {
                            demnha += 1;
                            if (demnha == 7)
                            {
                                addStatus(index, "Lỗi captcha !");
                                driver.Close();
                                driver.Dispose();
                                driver.Quit();
                                goto ketthuc;
                            }
                            goto lainha;
                        }

                        //Cảnh bảo lỗi
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(3));

                addStatus(index, "Đã login mail !");
                clickDeSau(driver); Thread.Sleep(TimeSpan.FromSeconds(5));
                if (driver.Url.Contains("inoptions/recovery-options-collection"))
                {
                    driver.Navigate().GoToUrl(urlLogin);
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                if (driver.Url.Contains("om/signin/v2/challenge/iap") || (driver.Url.Contains("gle.com/signin/rejected")))
                {
                    saveError(act, "Very phone");
                    addStatus(index, "very phone !");
                    driver.Close();
                    driver.Dispose();
                    driver.Quit();
                    goto ketthuc;
                }
                if (driver.Url.Contains(".com/interstitials/birthday") || (driver.Url.Contains("ogle.com/web/chip")) || (driver.Url.Contains("/info/unknownerror")))
                {
                    addStatus(index, "Vui lòng thêm ngày sinh");
                    addbirthday(driver);
                }
                if ((driver.Url.Contains("m/signin/v2/identifier")) || (driver.Url.Contains("ccounts.google.com/speedbump/idvreenable")) || (driver.Url.Contains("m/signin/v2/disabled/explanation")))
                {
                    saveError(act, "Đăng nhập không thành công!");
                    addStatus(index, "Đăng nhập không thành công");
                    driver.Close();
                    driver.Dispose();
                    driver.Quit();
                    goto ketthuc;
                }
                if (driver.Url.Contains("ogle.com/web/chip"))
                {
                    addbirthday(driver);
                }
            searchz:
                if (driver.Url.Contains("/signin/v2/challenge/pwd"))
                {
                    saveError(act, "Đăng nhập không thành công");
                    addStatus(index, "Đăng nhập không thành công");
                    driver.Close();
                    driver.Dispose();
                    driver.Quit();
                    goto ketthuc;
                }
                else
                {
                    acYoutube active = new acYoutube(driver);
                    String url = txtKeyword.Text;
                    try
                    {
                        driver.Url = url;
                    }
                    catch
                    {
                        driver.Close();
                        driver.Dispose();
                        driver.Quit();
                        goto ketthuc;
                    }
                    active.gotoHome();
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                addStatus(index, "timeout access google");
            }
        ketthuc:
            runNew(1);
            return;
        }

       
        void saveProfile(account taikhoan, string proxy = "")
        {
            while (true)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(proxy))
                    {
                        File.AppendAllText("data\\info_profile.txt", taikhoan.email + "|" + taikhoan.password + "|" + taikhoan.mail_kp + "\r\n");
                    }
                    else
                    {
                        File.AppendAllText("data\\info_profile.txt", taikhoan.email + "|" + taikhoan.password + "|" + taikhoan.mail_kp + "|" + proxy + "\r\n");
                    }
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }
        void runNew(int indexKichBan)
        {
            Thread st = new Thread(() =>
            {
                run(indexKichBan);
            });
            st.Start();
            st.IsBackground = true;
            lsThread.Add(st);
            Thread.Sleep(1000);
        }
        void clickGotit(UndetectChromeDriver driver)
        {
            try { driver.FindElement(By.XPath("//yt-upsell-dialog-renderer//tp-yt-paper-button//yt-formatted-string")).Click(); } catch { }
        }
        int countTimeAll = 0;
        void addLink(string link, int time)
        {

        }
        void save()
        {

        }
        string ConvertListToString()
        {
            string data = "";
            foreach (var item in lstLink.ToList())
            {
                data += item.link_yt + "|" + item.count + "\r\n";
            }
            return data;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        void savep()
        {
            try { File.WriteAllText("data\\phut.txt", countTimeAll.ToString()); } catch { Thread.Sleep(100); }
        }
        List<link> lstLink;

        private void btnStop_Click(object sender, EventArgs e)
        {
            checkopentab = !checkopentab;
            clearchrome();
        }
        void clearchrome()
        {

            Process[] chromeDriverProcesses = Process.GetProcessesByName("gpmdriver.exe");
            foreach (var chromeDriverProcess in chromeDriverProcesses)
            {
                try { chromeDriverProcess.Kill(); } catch { }
            }
            Process[] chromed = Process.GetProcessesByName("chrome");
            foreach (var chrome in chromed)
            {
                try { chrome.Kill(); } catch { }
            }

            Process.Start("taskkill", "/F /IM gpmdriver.exe");
            Process.Start("taskkill", "/F /IM chromedriver.exe");
        }
        bool checkopentab = true;

        private int iSoLuongEmail;
        private int iSoLuong;
        private int iSoLuongDangChay;
        private int iIndexDangChay;
        private string strKeyWork;
        private string strTenChannel;
        private int iSoLanMoLink = 0;


     


        private void btnStop_Click_1(object sender, EventArgs e)
        {
            clearchrome();
            foreach(Thread t in lsThread)
            {
                t.Abort();
            }
        }


        private void btnWait_Click(object sender, EventArgs e)
        {
            checkopentab = !checkopentab;
        }
        List<Thread> lsThread = new List<Thread>();
        private void button2_Click_1(object sender, EventArgs e)
        {
            iSoLuongDangChay = 0;
            iSoLuongEmail = dataGrid.Rows.Count;
            if (iSoLuongEmail == 0)
            {
                return;
            }
            if (iSoLuongEmail < nbThread.Value)
            {
                nbThread.Value = iSoLuongEmail;
            }
            iSoLuong = (int)nbThread.Value;
            iIndexDangChay = 0;
            strKeyWork = txtKeyword.Text;
           
            Thread st = new Thread(() =>
            {
                createThread(1);
            });
            st.IsBackground = true;
            st.Start();
            lsThread.Add(st);
            Thread.Sleep(1000);
        }

        private void sub_CheckedChanged(object sender, EventArgs e)
        {

        }
    }  
    }
