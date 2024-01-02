using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using DAL.Models;
using System.Text.RegularExpressions;

namespace DAL
{
    public class DbManager
    {

        // BRANCHES

        private motorentoContext db = new motorentoContext();

        // Get all branches from the database
        public List<Branch> GetAllBranches()
        {
            return db.Branches.ToList();
        }

        // Get a specific branch from the database, identified by id
        public Branch GetBranchById(int id)
        {
            Branch b = db.Branches.FirstOrDefault(t => t.Id == id);
            if (b == null) // Make sure the branch exists first, return an error if it doesn't
            {
                throw new Exception("Branch not found in database");
            }
            return b;
        }

        // Add a new branch to the database
        public void AddBranch(Branch b)
        {
            db.Branches.Add(b);
            db.SaveChanges();
        }

        // Edit a branch's details in the database, identified by id
        public Branch EditBranch(int id, Branch newBranchDetails)
        {
            Branch b = this.GetBranchById(id);
            if (b == null) // Make sure the branch exists first, return an error if it doesn't
            {
                throw new Exception("Branch not found in database");
            }
            b.Address = newBranchDetails.Address; // Update the branch that is in the database, with the new values
            b.Latitude = newBranchDetails.Latitude;
            b.Longitude = newBranchDetails.Longitude;
            b.BranchName = newBranchDetails.BranchName;
            db.SaveChanges();
            return b;
        }

        // Delete a specific branch from the database, identified by id
        public List<Branch> DeleteBranch(int id)
        {
            Branch b = this.GetBranchById(id);
            if (b == null) // Make sure the branch exists in the database, and return an error if it doesn't
            {
                throw new Exception("Branch not found in database");
            }
            db.Branches.Remove(b);
            db.SaveChanges();
            return db.Branches.ToList();
        }

        //CARS

        // Get all cars from the database
        public List<Car> GetAllCars()
        {
            return db.Cars.ToList();
        }

        // Get a specific car from the database, identified by id
        public Car GetCarById(int id)
        {
            Car c = db.Cars.FirstOrDefault(t => t.Id == id);
            if (c == null) // Make sure it exists first, return an error if it doesn't
            {
                throw new Exception("Car not found in database");
            }
            return c;
        }

        // Get relevant data about all cars marked as "in working order" from the database, for the purpose of viewing in the catalog page
        public List<WorkingCar> GetWorkingCars()
        {
            List<WorkingCar> CarsList = (from car in db.Cars            // The catalog page will include combined data from numerous tables for each car.
                                         join model in db.Models        // (Car ID, mileage, pic URL, license plate number, manufacturer name, model name,
                                         on car.ModelId equals model.Id // daily price, delay price per day, manufacture year, gear, branch address and branch name)
                                         join branch in db.Branches
                                         on car.Branch equals branch.Id
                                         where car.InWorkingOrder == true
                                         select new WorkingCar(car.Id, car.Mileage, car.Pic, car.LicensePlateNumber,
                                         model.Manufacturer, model.ModelName, model.DailyPrice, model.DelayPricePerDay,
                                         model.ManufactureYear, model.Gear, branch.Address, branch.BranchName)).ToList();
            return CarsList;
        }

        // Same as above, filtering out cars that are not available for rent between the two given dates, due to existing bookings during that period
        public List<WorkingCar> GetWorkingCarsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<WorkingCar> CarsList = this.GetWorkingCars(); // Get relevant catalog info for all working cars
            List<WorkingCar> FilteredCarsList = new List<WorkingCar>(); // Initialize new list to gather from the above only the cars that are available between the two given dates
            foreach (WorkingCar car in CarsList) // Loop through all working cars (former list)
            {
                List<Order> OrdersList = db.Orders.Where(t => t.CarId == car.Id && t.DateReturned == null && (!(t.StartDate > endDate || t.EndDate < startDate))).ToList();
                if (!(OrdersList.Any())) // For each car, see if any order exists for it that coincides with given date range and hasn't been ended by returning the car
                {
                    FilteredCarsList.Add(car); // If none exists, add that car to the outcoming list
                }
            }
            return FilteredCarsList;
        }

        // Add a new car to the database
        public Car AddCar(Car newCar)
        {

            if (db.Models.FirstOrDefault(t => t.Id == newCar.ModelId) == null) // Make sure the new car's model exists in the database
            {
                throw new Exception("Car model not found");
            }

            if (db.Branches.FirstOrDefault(t => t.Id == newCar.Branch) == null) // Make sure the new car's branch exists in the database
            {
                throw new Exception("Branch not found");
            }

            if (db.Cars.FirstOrDefault(t => t.LicensePlateNumber == newCar.LicensePlateNumber) != null) // Make sure the new car's license plate number doesn't already exist in the database's cars table
            {
                throw new Exception("License plate number taken! A different car with the same license plate number already exists");
            }

            db.Cars.Add(newCar); // Add the car to the database
            db.SaveChanges();
            return newCar;
        }

        // Edit a specific car's details in the database, identified by id
        public Car EditCar(int id, Car newCarDetails)
        {

            if (db.Models.FirstOrDefault(t => t.Id == newCarDetails.ModelId) == null) // Make sure the car's model exists in the database
            {
                throw new Exception("Car model not found in database");
            }

            if (db.Branches.FirstOrDefault(t => t.Id == newCarDetails.Branch) == null) // Make sure the car's branch exists in the database
            {
                throw new Exception("Branch not found in database");
            }

            if (db.Cars.FirstOrDefault(t => t.Id != newCarDetails.Id && t.LicensePlateNumber == newCarDetails.LicensePlateNumber) != null) // Make sure no other car with the same license plate number exists in the database
            {
                throw new Exception("License plate number taken! A different car with the same license plate number already exists");
            }

            Car c = this.GetCarById(id); // Retrieve the requested car
            if (c == null) // Make sure the car exists in the database, and throw an error if it doesn't
            {
                throw new Exception("Car not found in database");
            }
            c.ModelId = newCarDetails.ModelId; // Update its details
            c.Mileage = newCarDetails.Mileage;
            c.Pic = newCarDetails.Pic;
            c.InWorkingOrder = newCarDetails.InWorkingOrder;
            c.LicensePlateNumber = newCarDetails.LicensePlateNumber;
            c.Branch = newCarDetails.Branch;
            db.SaveChanges(); // Save changes to database
            return c;
        }

        // Delete a specific car from the database, identified by id
        public List<Car> DeleteCar(int id)
        {
            Car c = this.GetCarById(id);
            if (c == null) // Make sure the car exists in the database, and throw an error if it doesn't
            {
                throw new Exception("Car not found in database");
            }
            db.Cars.Remove(c);
            db.SaveChanges();
            return db.Cars.ToList();
        }

        //MODELS

        // Get all car models from the database
        public List<Model> GetAllModels()
        {
            return db.Models.ToList();
        }

        // Get a specific car model from the database, identified by id
        public Model GetModelById(int id)
        {
            Model m = db.Models.FirstOrDefault(t => t.Id == id);
            if (m == null) // Make sure the car model exists in the database, and throw an error if it doesn't
            {
                throw new Exception("Model not found in database");
            }
            return m;
        }

        // Add a new car model to the database
        public void AddModel(Model m)
        {
            db.Models.Add(m);
            db.SaveChanges();
        }

        // Edit a car model's details in the database, identified by id
        public Model EditModel(int id, Model newModelDetails)
        {
            Model m = this.GetModelById(id);
            if (m == null) // Make sure the car model exists in the database, and throw an error if it doesn't
            {
                throw new Exception("Model not found in database");
            }
            m.Manufacturer = newModelDetails.Manufacturer; // Update selected model's details
            m.ModelName = newModelDetails.ModelName;
            m.DailyPrice = newModelDetails.DailyPrice;
            m.DelayPricePerDay = newModelDetails.DelayPricePerDay;
            m.ManufactureYear = newModelDetails.ManufactureYear;
            m.Gear = newModelDetails.Gear;
            db.SaveChanges(); // Save changes to the database
            return m;
        }

        // Delete a car model from the database, identified by id
        public List<Model> DeleteModel(int id)
        {
            Model c = this.GetModelById(id);
            if (c == null) // Make sure the car model exists in the database, and return an error if it doesn't
            {
                throw new Exception("Car model not found in the database");
            }
            db.Models.Remove(c);
            db.SaveChanges();
            return db.Models.ToList();
        }

        //ORDERS

        // Get all orders from the database
        public List<Order> GetAllOrders()
        {
            return db.Orders.ToList();
        }

        // Get a single order's data from the database, identified by id
        public Order GetOrderById(int id)
        {
            Order d = db.Orders.FirstOrDefault(t => t.Id == id);
            if (d == null) // Make sure the order exists in the database, and throw an error if it doesn't
            {
                throw new Exception("Order not found in database");
            }
            return d;
        }

        //Get all orders made by a specific user, identified by said user's user-name
        public List<Order> GetOrdersByUserName(string username)
        {
            List<Order> orderList = db.Orders.Where(t => t.UserName == username).ToList();
            return orderList;
        }

        //Get an order's car model, identified by the model id
        public Model GetModelFromOrder(int id)
        {
            Order d = db.Orders.FirstOrDefault(t => t.Id == id);
            if (d == null) throw new Exception("Order id not found in database"); // Make sure the order exists in the database, and throw an error if it doesn't
            Car c = db.Cars.FirstOrDefault(t => t.Id == d.CarId);
            if (c == null) throw new Exception("Order's car not found in database"); // Make sure the order's car exists in the database, and throw an error if it doesn't
            Model m = db.Models.FirstOrDefault(t => t.Id == c.ModelId);
            if (m == null) throw new Exception("Order's car model not found in database"); // Make sure the order's car model exists in the database, and throw an error if it doesn't
            return m;
        }

        // Get relevant data about every order made by a specific user, (identified by user name) for viewing in that user's order-history page
        public List<HistoryOrder> GetHistoryOrdersByUserName(string username)
        {
            List<Order> userOrders = this.GetOrdersByUserName(username); // Get all orders made by the specified user
            List<HistoryOrder> historyOrders = new List<HistoryOrder>(); // Initialize a list, into which we will gather all the relevant data about orders
                                                                         // for viewing in the user's order-history page (Joint data from numerous tables)
            foreach (Order order in userOrders) // Loop through all the orders made by the specified user
            {
                Model orderModel; // This variable will include each loop pass's order's car model

                try
                {
                    orderModel = this.GetModelFromOrder(order.Id); // Get the order's car's model from the database
                }
                catch
                {
                    continue; // If no car model was found for the order, carry on looping through the other orders left in the list
                }

                Decimal totalCost = orderModel.DailyPrice * (Decimal)(order.EndDate - order.StartDate).TotalDays; // Calculate the total cost of the order (by daily price and date range, not including delay fees)
                if (order.DateReturned != null && order.DateReturned > order.EndDate) // If in this order the car was returned later than due,
                {
                    totalCost += orderModel.DelayPricePerDay * (Decimal)((DateTime)order.DateReturned - order.EndDate).TotalDays; // Add the delay fees to the total cost calculation
                }

                HistoryOrder historyOrder; // This variable will gather the necessary information about each loop pass's order, for viewing in the user's order history page

                if (order.DateReturned.HasValue) // If the car has been returned, include in said information the date in which the car was actually returned
                {
                    historyOrder = new HistoryOrder(order.Id, order.StartDate, order.EndDate, totalCost, orderModel.Manufacturer, orderModel.ModelName, (DateTime)order.DateReturned);
                }
                else // If it wasn't returned, leave the field "date returned" blank
                {
                    historyOrder = new HistoryOrder(order.Id, order.StartDate, order.EndDate, totalCost, orderModel.Manufacturer, orderModel.ModelName);
                }

                historyOrders.Add(historyOrder); // Add the current loop pass's relevant order information to the outgoing list

            }

            return historyOrders; // return all orders with the relevant information for order-history page

        }

        // Create new order in database
        public void AddOrder(Order newOrder)
        {

            if (db.Users.FirstOrDefault(t => t.UserName == newOrder.UserName) == null) // Make sure the order's user name exists in the database, and throw an error if it doesn't
            {
                throw new Exception("User name not found");
            }

            if (db.Cars.FirstOrDefault(t => t.Id == newOrder.CarId) == null) // Make sure the order's car ID exists in the database, and throw an error if it doesn't
            {
                throw new Exception("Car not found");
            }

            if (newOrder.StartDate >= newOrder.EndDate) // Make sure the order's return date comes at least 1 day after its rent date, and throw an error if it isn't
            {
                throw new Exception("Return date must be at least 1 day after rent date");
            }

            List<Order> OrdersList = db.Orders.Where(t => t.DateReturned == null && t.CarId == newOrder.CarId).ToList(); // Gather all of this order's car's unreturned rents that may be

            foreach (Order item in OrdersList) // Make sure the new incoming order does not overlap with any another unreturned order of the same car.
            {                                  // In simple words, make sure the car is available for rent in the requested date range. Throw an error otherwise.
                if (newOrder.StartDate <= item.EndDate)
                {
                    if (newOrder.StartDate >= item.StartDate)
                    {
                        throw new Exception("This car is not available for the requested time period. An overlapping order has already been made");
                    }
                    else
                    {
                        if (newOrder.EndDate >= item.StartDate)
                        {
                            throw new Exception("This car is not available for the requested time period. An overlapping order has already been made");
                        }
                    }
                }
            }

            Car c = this.GetCarById(newOrder.CarId);
            db.Orders.Add(newOrder);
            db.SaveChanges();
        }

        // Perform car return procedure, including updating order as returned at current date
        public Order Return(Car returningCar)
        {
            Regex rgx = new Regex("^[0-9]{3,9}$");
            if (!(rgx.IsMatch(returningCar.LicensePlateNumber))) // Make sure the input license plate number consists if only between 3 and 9 digits and nothing else
            {
                throw new Exception("License plate number must consist of only between 3 and 9 digits");
            }
            Car c = db.Cars.FirstOrDefault(t => t.LicensePlateNumber == returningCar.LicensePlateNumber); // Get the corresponding car with the matching license plate number
            if (c == null) // Make sure the car exists in the database
            {
                throw new Exception("Requested car was not found"); // If it doesn't, throw an error
            }
            DateTime current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); // Get the current date
            List<Order> orderList = db.Orders.Where(t => t.CarId == c.Id && t.DateReturned == null && t.StartDate <= current).ToList(); // Get all unreturned orders of this car whose start date is not in the future
            if (!orderList.Any()) // If non exists, there isn't an order to end,
            {
                throw new Exception("No ongoing rent was found for the requested car"); // so throw an error
            }
            Order d = orderList.OrderByDescending(t => t.Id).FirstOrDefault(); // If by mistake somehow more than one such order exists in the database, take the latest one
            d.DateReturned = current; // And mark that order as returned today
            db.SaveChanges(); // Save changes to the database
            return d;
        }

        // Edit a specific order's details in database, identifying the order by its id
        public Order EditOrder(int id, Order newOrderDetails)
        {

            if (db.Users.FirstOrDefault(t => t.UserName == newOrderDetails.UserName) == null) // Make sure the order's user name exists in the database
            {
                throw new Exception("User name not found"); // Throw an error if it doesn't
            }

            if (db.Cars.FirstOrDefault(t => t.Id == newOrderDetails.CarId) == null) // Make sure the order's car exists in the database
            {
                throw new Exception("Car not found"); // Throw an error if it doesn't
            }

            if (newOrderDetails.EndDate <= newOrderDetails.StartDate) // Make sure the order's return date is at least 1 day later than the rent date
            {
                throw new Exception("Return date must be at least 1 day after rent date"); // Throw an error if it doesn't
            }

            if (newOrderDetails.DateReturned != null && newOrderDetails.DateReturned < newOrderDetails.StartDate) // If, according to the new update details, the order has been returned,
            {                                                                                                     // make sure the date in which it was returned doesn't percede that rent date
                throw new Exception("Date returned cannot precede rent date"); // Throw an error if it does
            }

            List<Order> OrdersList = db.Orders.Where(t => t.DateReturned == null && t.CarId == newOrderDetails.CarId && t.Id != newOrderDetails.Id).ToList(); // Gather all of this order's car's unreturned rents that may be
                                                                                                                                                              // (not including this one we are editing, of course)
            foreach (Order item in OrdersList) // Make sure that with the new order's details, the order does not overlap with another order of the same car.
            {                                  // In simple words, make sure the car is available for rent in the requested date range. Throw an error otherwise.
                if (newOrderDetails.StartDate <= item.EndDate)
                {
                    if (newOrderDetails.StartDate >= item.StartDate)
                    {
                        throw new Exception("This car is not available for the requested time period. An overlapping order has already been made");
                    }
                    else
                    {
                        if (newOrderDetails.EndDate >= item.StartDate)
                        {
                            throw new Exception("This car is not available for the requested time period. An overlapping order has already been made");
                        }
                    }
                }
            }

            Order d = this.GetOrderById(id);
            d.StartDate = newOrderDetails.StartDate; // Update the order's details
            d.EndDate = newOrderDetails.EndDate;
            d.DateReturned = newOrderDetails.DateReturned;
            d.UserName = newOrderDetails.UserName;
            d.CarId = newOrderDetails.CarId;
            db.SaveChanges(); // Save changes in the database
            return d;
        }

        // Delete a specific order from the database, identified by id
        public List<Order> DeleteOrder(int id)
        {
            Order d = this.GetOrderById(id); // Retrieve order
            if (d == null) // Make sure the order exists in the database
            {
                throw new Exception("Order not found in the database"); // If it doesn't, throw an error
            }
            db.Orders.Remove(d); // Delete the order
            db.SaveChanges(); // Save changes in the database
            return db.Orders.ToList();
        }

        //USERS

        // Get all users from database
        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        //Get a single user from database, by user name
        public User GetUserByUserName(string username)
        {
            User u = db.Users.FirstOrDefault(t => t.UserName == username);
            if (u == null) // Make sure the user exists in the database
            {
                throw new Exception("User not found in database"); // Throw an error otherwise
            }
            return u;
        }

        // Register a new user
        public void Register(User newUser)
        {
            User u = db.Users.FirstOrDefault(t => t.UserName == newUser.UserName);
            if (u != null) // Make sure a user with the same user name doesn't already exists in the database
            {
                throw new Exception("A user with the same user name already exists"); // Throw an error otherwise
            }
            db.Users.Add(newUser); // Add the new user to the database
            db.SaveChanges(); // Save changes in the database
        }


        // As part of the user login process, verify the user name and password, and get the user's role for clearance purposes
        public string Login(User loginDetails)
        {

            User u = db.Users.FirstOrDefault(t => t.UserName == loginDetails.UserName); // Retrieve the corresponding user from the database by, user name

            if (u == null || u.Password != loginDetails.Password) // Make sure the password is correct
            {
                throw new Exception("Wrong user name and/or password"); // Throw an error otherwise
            }

            return u.Role; // Return the user's role for clearance purposes

        }

        // Edit a single user's data in the database
        public User EditUser(User newUserDetails)
        {
            User u = db.Users.FirstOrDefault(t => t.UserName == newUserDetails.UserName); // Find an existing user with the same user name in the database

            if (u == null) // If none was found,
            {
                throw new Exception("User name not found"); // Throw an error
            }

            u.Password = newUserDetails.Password; // Update the user's data in the database
            u.Role = newUserDetails.Role;
            u.FirstName = newUserDetails.FirstName;
            u.LastName = newUserDetails.LastName;
            u.BirthDate = newUserDetails.BirthDate;
            u.Sex = newUserDetails.Sex;
            u.Email = newUserDetails.Email;
            u.Pic = newUserDetails.Pic;
            db.SaveChanges(); // Save changes to the database
            return newUserDetails;
        }

        // Delete a single user from the database, identified by user name
        public List<User> DeleteUser(string username)
        {
            User u = db.Users.FirstOrDefault(t => t.UserName == username); // Get the user from the database with the matching user name

            if (u == null) // If it doesn't exist in the database,
            {
                throw new Exception("User name not found"); // Throw an error
            }

            db.Users.Remove(u); // Delete the user from the database
            db.SaveChanges(); // Save changes to the database
            return db.Users.ToList();
        }

    }
}
