using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace ContactManager
{
    public partial class GitCommit : Form
    {
        private string _gitRepoPath = Sqlite.LoadDBDir();
        private string _gitName = "gitUsername";
        private string _gitEmail = "gitemail@gmail.com";

        public GitCommit()
        {
            InitializeComponent();
        }

        private void AppendLog(string log)
        {
            textLog.Text += log + Environment.NewLine; 
        }

        #region [ Git ]
        public void StageChanges()
        {
            using (var repo = new Repository(_gitRepoPath))
            {
                try
                {
                    RepositoryStatus status = repo.RetrieveStatus();
                    List<string> filePaths = status.Modified.Select(mods => mods.FilePath).ToList();
                    filePaths.AddRange(status.Untracked.Select(mods => mods.FilePath).ToList());
                    Commands.Stage(repo, filePaths);
                }
                catch (Exception ex)
                {
                    AppendLog("Exception:StageChanges " + ex.Message);
                }
            }
        }
        public void CommitChanges()
        {
            using (var repo = new Repository(_gitRepoPath))
            {
                try
                {
                    repo.Commit(textComment.Text, new Signature(_gitName, _gitEmail, DateTimeOffset.Now),
                        new Signature(_gitName, _gitEmail, DateTimeOffset.Now));
                }
                catch (Exception e)
                {
                    AppendLog("Exception:CommitChanges " + e.Message);
                }
            }
        }

        public void PushChanges()
        {
            using (var repo = new Repository(_gitRepoPath))
                try
                {
                    var remote = repo.Network.Remotes["origin"];
                    var pushOptions = new PushOptions
                    {
                        CredentialsProvider = (_url, _user, _cred) =>
                   new UsernamePasswordCredentials { Username = "validuser", Password = "validpassword" }
                    };

                    string pushRefSpec = string.Format("+{0}:{0}", "refs/heads/" + comboBranch.SelectedItem);
                    repo.Network.Push(remote, pushRefSpec, pushOptions);
                }
                catch (Exception e)
                {
                    AppendLog("Exception:PushChanges " + e.Message);
                }
        } 
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            StageChanges();
            CommitChanges();
            PushChanges();
        }

        private void GitCommit_Load(object sender, EventArgs e)
        {
            if (!Repository.IsValid(_gitRepoPath))
            {
                Repository.Init(_gitRepoPath);
                using (var repo = new Repository(_gitRepoPath))
                {
                    StageChanges();
                    CommitChanges();

                    var branches = repo.Branches;
                    foreach (var b in branches)
                    {
                        comboBranch.Items.Add(b.FriendlyName);
                    }
                }
            }
            else
            {
                using (var repo = new Repository(_gitRepoPath))
                {

                    var branches = repo.Branches;
                    foreach (var b in branches)
                    {
                        comboBranch.Items.Add(b.FriendlyName);
                    }
                    if (branches.Count() > 0)
                    {
                        comboBranch.SelectedIndex = 0;
                    }
                }
                
            }
        }
    }
}
