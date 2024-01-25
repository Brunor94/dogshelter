# Dog Shelter

A Checkmarx SCA coding exercise.


## Requirements

1. **We prefer C#, but other languages are also fine**

   If you prefer to implement it in a different language, let us know beforehand.

2. **It's not a riddle**

   If something isn't clear, ask us. Don't waste time on figuring out or guess our intentions when they're not obvious.
   We appreciate feedback.

4. **The code should be clean and readable**

   There's no need for over-engineering but we do wish to see a good separation of concerns. The code should be simple, clean and testable. It should align with the SOLID principles.

5. **It needs to work**

   We want to see a working solution, even if it's not complete. A compiled and running code with missing features is
   better than a solution that doesn't compile or errors out.

6. **Use in-memory storage**

   Do not setup a real database. Use static members or any in-memory storage library. Still, adding a persistancy solution shouldn't require a complete refactor of your project.


## Specifications

We want to build a **REST API** for a dog shelter. Users will be able to register dogs in the system, and query available
dogs by size category.


### Register a new dog

A user should be able to add a new dog to the system. The new dog request should include the dog's name and breed.

Before saving the dog in the in-memory storage, the service should find the dog's breed in the following public dog API:

    https://api.thedogapi.com/v1/breeds/search?q=canaan

This third-party API returns a metric height (minimum and maximum). You should calculate the average - in this example the average height is 54 (`(61+48)/2`).

```json
[
  {
    "weight": {
      "imperial": "35 - 55",
      "metric": "16 - 25"
    },
    "height": {
      "imperial": "19 - 24",
      "metric": "48 - 61"
    },
    "id": 66,
    "name": "Canaan Dog",
    "bred_for": "Guarding flocks and encampments",
    "breed_group": "Herding",
    "life_span": "12 - 14 years",
    "temperament": "Cautious, Devoted, Alert, Quick, Intelligent, Vigilant",
    "reference_image_id": ""
  }
]
```

The average height should be stored together with the dog name and breed.


### Find an available dog by size

A user should be able to find all dogs of a certain size category. The size categories are:

- Small: Smaller than 35cm
- Medium: Between 35cm and 55cm
- Large: Bigger than 55cm

For example, by querying all dogs of the small size category, I should see the dog Pugster in the response, if Pugster
was registered as a Pug breed.

### Write unit tests

Cover your code with unit tests as you see fit.


### Bonus: Find dogs by breed and temperament 

> **This is an optional bonus question** - We respect your free time and do not require or expect you to complete this bonus question. It is only provided as an oppurtinity for you to "show-off" your coding and design skills if you choose to do so.

At this point users are able to find dogs by their size. Let's give them the ability to search for dogs by other criteria. There's no need to support multiple filter (e.g. size and breed) in the same request.

1. Add the capability to filter dogs by their breed.
2. Add the capability to filter dogs by their temperament. In the example above, requests for a dog with a temperament of "Vigilant" should return Canaan dogs. As you can see in the public dog API, each dog can have several temperaments.
