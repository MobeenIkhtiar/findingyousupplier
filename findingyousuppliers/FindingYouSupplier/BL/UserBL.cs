using FindingYouSupplier.DAL;
using FindingYouSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindingYouSupplier.BL
{
    public class UserBL
    {

        #region User
        public List<User> getUserList()
        {
            return new UserDAL().getUsersList();
        }

        public User getUserById(int _id)
        {
            return new UserDAL().getUserById(_id);
        }

        public bool AddUser(User _user)
        {
            if (_user.Name == null || _user.Email == null || _user.Password == null || _user.Website_Address == null || _user.Phone == null)
                return false;
            return new UserDAL().AddUser(_user);
        }

        public bool UpdateUser(User _user)
        {
            if (_user.Name == null || _user.Email == null || _user.Password == null || _user.Website_Address == null || _user.Phone == null)
                return false;

            return new UserDAL().UpdateUser(_user);
        }

        //public void DeleteUser(int _id)
        //{
        //    new UserDAL().DeleteUser(_id);
        //}
        #endregion

        #region Product
        public List<Product> getProductList()
        {
            return new UserDAL().getProductsList();
        }

        public Product getProductById(int _id)
        {
            return new UserDAL().getProductById(_id);
        }

        public bool AddProduct(Product _user)
        {
            if (_user.Name == null || _user.Minimum_Order == null || _user.Delievery_Charges == null)
                return false;
            return new UserDAL().AddProduct(_user);
        }

        public bool UpdateProduct(Product _user)
        {
            if (_user.Name == null || _user.Minimum_Order == null || _user.Delievery_Charges == null)
                return false;

            return new UserDAL().UpdateProduct(_user);
        }
        #endregion

        #region Category
        public List<Category> getCategoryList()
        {
            return new UserDAL().getCategoriesList();
        }

        public Category getCategoryById(int _id)
        {
            return new UserDAL().getCategoryById(_id);
        }

        public bool AddCategory(Category _user)
        {
            if (_user.Name == null)
                return false;
            return new UserDAL().AddCategory(_user);
        }

        public bool UpdateCategory(Category _user)
        {
            if (_user.Name == null)
                return false;

            return new UserDAL().UpdateCategory(_user);
        }
        #endregion

        #region Image
        public List<Image> getImageList()
        {
            return new UserDAL().getImagesList();
        }

        public Image getImageById(int _id)
        {
            return new UserDAL().getImageById(_id);
        }

        public bool AddImage(Image _user)
        {
            if (_user.Path == null)
                return false;

            return new UserDAL().AddImage(_user);
        }

        public bool UpdateImage(Image _user)
        {
            if (_user.Path == null)
                return false;

            return new UserDAL().UpdateImage(_user);
        }
        #endregion

        #region Message
        public List<Message> getMessageList()
        {
            return new UserDAL().getMessagesList();
        }

        public Message getMessageById(int _id)
        {
            return new UserDAL().getMessageById(_id);
        }

        public bool AddMessage(Message _user)
        {
            if (_user.From_User_Id == null || _user.To_User_Id == null || _user.Msg == null || _user.Date == null)
                return false;
            return new UserDAL().AddMessage(_user);
        }
        #endregion
    }
}