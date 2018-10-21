using FindingYouSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindingYouSupplier.DAL
{
    public class UserDAL
    {

        DatabaseEntities db;

        #region User
        public List<User> getUsersList()
        {
            List<User> users;
            using (db = new DatabaseEntities())
            {
                users = db.Users.ToList();
            }

            return users;
        }

        public User getUserById(int _Id)
        {
            User user;
            using (db = new DatabaseEntities())
            {
                user = db.Users.FirstOrDefault(x => x.Id == _Id);
            }

            return user;
        }

        public bool AddUser(User _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Users.Add(_user);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateUser(User _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        //public void DeleteUser(int _id)
        //{
        //    using (db = new DatabaseEntities())
        //    {
        //        db.Users.Remove(db.Users.FirstOrDefault(x => x.Id == _id));
        //        db.SaveChanges();
        //    }
        //}
        #endregion

        #region Product
        public List<Product> getProductsList()
        {
            db = new DatabaseEntities();
            List<Product> users;
            //using (db = new DatabaseEntities())
            //{
                users = db.Products.ToList();
            //}

            return users;
        }

        public Product getProductById(int _Id)
        {
            db = new DatabaseEntities();
            Product user;
            //using (db = new DatabaseEntities())
            //{
                user = db.Products.FirstOrDefault(x => x.Id == _Id);
            //}

            return user;
        }

        public bool AddProduct(Product _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Products.Add(_user);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateProduct(Product _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }
        #endregion

        #region Category
        public List<Category> getCategoriesList()
        {
            List<Category> users;
            using (db = new DatabaseEntities())
            {
                users = db.Categories.ToList();
            }

            return users;
        }

        public Category getCategoryById(int _Id)
        {
            Category user;
            using (db = new DatabaseEntities())
            {
                user = db.Categories.FirstOrDefault(x => x.Id == _Id);
            }

            return user;
        }

        public bool AddCategory(Category _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Categories.Add(_user);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateCategory(Category _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }
        #endregion

        #region Image
        public List<Image> getImagesList()
        {
            List<Image> users;
            using (db = new DatabaseEntities())
            {
                users = db.Images.ToList();
            }

            return users;
        }

        public Image getImageById(int _Id)
        {
            Image user;
            using (db = new DatabaseEntities())
            {
                user = db.Images.FirstOrDefault(x => x.Id == _Id);
            }

            return user;
        }

        public bool AddImage(Image _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Images.Add(_user);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateImage(Image _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }
        #endregion

        #region Message
        public List<Message> getMessagesList()
        {
            db = new DatabaseEntities();
            List<Message> users;
            //using (db = new DatabaseEntities())
            //{
                users = db.Messages.ToList();
            //}

            return users;
        }

        public Message getMessageById(int _Id)
        {
            db = new DatabaseEntities();
            Message user;
            //using (db = new DatabaseEntities())
            //{
                user = db.Messages.FirstOrDefault(x => x.Id == _Id);
            //}

            return user;
        }

        public bool AddMessage(Message _user)
        {
            using (db = new DatabaseEntities())
            {
                db.Messages.Add(_user);
                db.SaveChanges();
            }
            return true;
        }
        #endregion


    }
}