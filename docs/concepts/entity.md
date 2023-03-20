# What is an entity?

An entity is an object, person, place, thing, or concept that can be distinctly and clearly identified in a system or data model. In the context of databases and information management systems, an entity is often used to represent a class of objects or concepts, such as customers, products, sales, or financial transactions.

Entities are typically defined through a set of attributes that describe their characteristics and properties. For example, an "customer" entity may have attributes such as name, address, phone number, and email address. Each instance or occurrence of this entity would have specific values for these attributes, such as "John Smith", "123 Main St", "(555) 555-5555", and "john.smith@email.com".

Entities can be related to each other through relationships, which are used to describe how entities interact with each other. For example, a "order" entity may be related to a "customer" entity through a "placed" relationship, indicating that the order was placed by the customer.

## Entity example

```csharp
public class Customer : Entity, IAggregateRoot
{
    protected Customer() { }

    public Customer(string name, string address, string phoneNumber, string emailAddress)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
    }

    public string Name { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public string EmailAddress { get; private set; }
}
```

## What is aggregation root?

The aggregate root is an important concept in software domain modeling. It is an entity that is responsible for a set of related objects, and it is the only entity that is referenced by other entities outside the scope of the set. In other words, the aggregate root is the main entity that contains and manages a set of related entities.

In practice, the aggregation root is a class that encapsulates related classes, which are called aggregates. This structure allows the aggregate classes to be managed as a cohesive set, with the aggregation root being responsible for ensuring that business rules are correctly applied throughout the set.

A common example of an aggregation root is a purchase order in an e-commerce system. The order would be the aggregation root, with aggregates such as purchase items, payment information, and shipping information. The aggregation root would be responsible for ensuring that all the information related to the order is correct and that business rules, such as payment restrictions and shipping calculations, are correctly applied throughout the set.

In summary, the aggregation root is a primary entity that contains and manages a set of related entities, called aggregates. The aggregation root is responsible for ensuring that business rules are applied correctly throughout the set and is the only entity referenced by other entities outside the scope of the set. The use of this concept is essential for the development of more cohesive and scalable software systems.
