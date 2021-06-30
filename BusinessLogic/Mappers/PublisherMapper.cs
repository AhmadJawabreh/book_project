using Entities;
using Models;
using Resources;
using System.Collections.Generic;

namespace BusinessLogic.Mappers
{
   public  class PublisherMapper
    {    
       
        public static List<PublisherResource> ToResources(IEnumerable<Publisher> Publishers)
        {
            List<PublisherResource> publisherResources = new List<PublisherResource>();
            foreach (var Item in Publishers)
            {
                publisherResources.Add(new PublisherResource
                    {
                        Id = Item.Id,
                        FirstName = Item.FirstName,
                        LastName = Item.LastName,
                        Email = Item.Email,
                        Address = Item.Address,
                        Phone = Item.Phone
                    }
                );
            }
            return publisherResources;
        }

        public static PublisherResource ToResource(Publisher publisher) 
        {
            PublisherResource publisherResource = new PublisherResource();
            publisherResource.Id = publisher.Id;
            publisherResource.FirstName = publisher.FirstName;
            publisherResource.LastName = publisher.LastName;
            publisherResource.Email = publisher.Email;
            publisherResource.Address = publisher.Address;
            publisherResource.Phone = publisher.Phone;
            return publisherResource;
        }

        public static Publisher ToEntity(Publisher publisher,PublisherModel publisherModel)
        {
            publisher.FirstName = publisherModel.FirstName.Trim();
            publisher.LastName = publisherModel.LastName.Trim();
            publisher.Email = publisherModel.Email?.Trim();
            publisher.Phone = publisherModel.Phone?.Trim();
            publisher.Address = publisherModel.Address?.Trim();
            return publisher;
        }

        public static PublisherModel ToModel(PublisherResource publisherResource)
        {
            PublisherModel publisherModel = new PublisherModel();
            publisherModel.Id = (int) publisherResource.Id;
            publisherModel.FirstName = publisherResource.FirstName.Trim();
            publisherModel.LastName = publisherResource.LastName.Trim();
            publisherModel.Email = publisherResource.Email?.Trim();
            publisherModel.Phone = publisherResource.Phone?.Trim();
            publisherModel.Address = publisherResource.Address?.Trim();
            return publisherModel;
        }


    }
}
