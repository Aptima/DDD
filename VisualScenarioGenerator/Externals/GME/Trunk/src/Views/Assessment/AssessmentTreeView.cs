using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace AME.Views.Assessment
{
    public class AssessmentTreeView : TreeView, IAssessmentView
    {
        private IAssessmentViewHelper myHelper;
        private Boolean updating, found;
        private TreeNode currentlyChecked;
        private String checkedName;
        private String terminatingParentName;

        public TreeNode CheckedNode
        {
            get { return currentlyChecked; }
        }

        public String HierarchyTop { set { terminatingParentName = value; } }

        public Boolean Updating { get { return updating; } }

        public IAssessmentViewHelper Helper
        {
            get
            {
                return myHelper;
            }
            set
            {
                myHelper = value;
            }
        }

        public AssessmentTreeView()
            : base()
        {
            myHelper = new DefaultViewHelper();
            this.BeforeCheck += new TreeViewCancelEventHandler(AssessmentTreeView_BeforeCheck);
            this.AfterCheck += new TreeViewEventHandler(AssessmentTreeView_AfterCheck);
        }

        public void ClearCheckedState()
        {
            currentlyChecked = null;
            checkedName = null;
        }

        public TreeNode SearchTree(TreeNodeCollection nodes, String text)
        {
            TreeNode ret = null;
            foreach (TreeNode node in nodes)
            {
                if (node.Text == text)
                {
                    return node;
                }
                ret = SearchTree(node.Nodes, text);
                if (ret != null)
                {
                    return ret;
                }
            }
            return ret;
        }

        void AssessmentTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (!updating && currentlyChecked != null && e.Node != currentlyChecked)
            {
                updating = true;
                // find the name
                TreeNode find = SearchTree(this.Nodes, checkedName);
                if (find != null)
                {
                    find.Checked = false;
                    if (currentlyChecked.Checked != find.Checked)
                    {
                        currentlyChecked = find;
                    }
                }

                updating = false;
            }
        }

        private void AssessmentTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!updating)
            {
                currentlyChecked = e.Node;
                checkedName = currentlyChecked.Text;
            }
        }

        public void Populate()
        {
            updating = true;

            this.Nodes.Clear();

            List<XmlNode> items = myHelper.GetData(); ;

            found = false;
            if (items != null)
            {
                if (items.Count > 0)
                {
                    found = false;

                    if (terminatingParentName == null)
                    {
                        foreach (XmlNode child in items)
                        {
                            String nameAttr = child.Attributes["name"].Value;
                            TreeNode tchild = this.Nodes.Add(nameAttr);
                            if (nameAttr == checkedName)
                            {
                                tchild.Checked = true;
                                currentlyChecked = tchild;
                                checkedName = tchild.Text;
                                found = true;
                            }
                        }
                    }
                    else
                    {
                        XmlNode first = items[0];
                        while (first.ParentNode != null && first.ParentNode.Attributes["name"].Value != terminatingParentName)
                        {
                            first = first.ParentNode;
                        }

                        String nameAttr = first.Attributes["name"].Value;
                        TreeNode tchild = this.Nodes.Add(nameAttr);
                        if (nameAttr == checkedName)
                        {
                            tchild.Checked = true;
                            currentlyChecked = tchild;
                            checkedName = tchild.Text;
                            found = true;
                        }

                        AddNodes(tchild.Nodes, first);
                    }
                    if (!found)
                    {
                        checkedName = null;
                        currentlyChecked = null;
                    }
                }
            }
            updating = false;
        }

        private void AddNodes(TreeNodeCollection nodes, XmlNode parent)
        {
            XmlNodeList list = parent.SelectNodes("DataElement");
            foreach (XmlNode child in list)
            {
                String nameAttr = child.Attributes["name"].Value;
    
                TreeNode tchild = nodes.Add(nameAttr);
                if (nameAttr == checkedName)
                {
                    tchild.Checked = true;
                    found = true;
                }
                AddNodes(tchild.Nodes, child);
            }
        }
    }
}
