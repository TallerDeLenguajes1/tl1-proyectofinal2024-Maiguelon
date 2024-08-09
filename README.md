# Torneo de Candidatos

## Descripción del Proyecto

**Torneo de Candidatos** es un juego de simulación de combate por turnos, ambientado en un torneo ficticio. Los personajes luchan por obtener el trono en un combate a muerte. El juego incluye una amplia variedad de personajes, cada uno con habilidades y ataques únicos.

## Características Principales

- **Generación de Personajes Aleatoria:** Los personajes se generan aleatoriamente utilizando una API externa que proporciona nombres, junto con epítetos y clases que varían según el tipo de personaje (Mago, Guerrero, Pícaro, Druida).
- **Sistema de Combate:** Los combates se realizan por turnos, con diferentes ataques y hechizos que se aplican según la clase del personaje.
- **Historial de Ganadores:** Se guarda un historial de ganadores, con el nombre y epíteto del personaje que ha ganado cada torneo.
- **Persistencia de Partidas:** Las partidas se pueden guardar y cargar, permitiendo continuar un torneo en progreso.

## Tecnologías Utilizadas

- **Lenguaje:** C#
- **Desarrollo Asíncrono:** El juego utiliza tareas asíncronas para operaciones como la generación de personajes y la ejecución de combates.
- **Persistencia de Datos:** JSON se utiliza para guardar y leer datos persistentes, como los ganadores del torneo y las partidas guardadas.
- **Manipulación de la Consola:** Se emplea la consola para la interacción del usuario, con personalización de colores y centrado de texto para mejorar la experiencia visual.

## USO

Al comenzar el juego, se presentará un menú con distintas opciones para el usuario:

**1-Nueva partida** 

Corre el torneo desde 0 hasta que se termina o se guarda. 

**2-Cargar partida**

Carga la partida si fue guardada anteriormente.

**3-Mostrar ganadores**

Muestra un histórico de los ganadores, escritos en color según su clase.

**4-Salir**

Termina el programa.

*Si se ingresa algo distinto a estos 4 numeros pedirá ingresar de nuevo*

## API

**RandomUser** es la API que utilizo, me da multiples datos de los cuales uso solo el nombre para mis personajes.

La API devuelve algo así:

{
  "results": [
    {
      "gender": "female",
      "name": {
        "title": "Miss",
        "first": "Jennie",
        "last": "Nichols"
      },
      "location": {
        "street": {
          "number": 8929,
          "name": "Valwood Pkwy",
        },
        "city": "Billings",
        "state": "Michigan",
        "country": "United States",
        "postcode": "63104",
        "coordinates": {
          "latitude": "-69.8246",
          "longitude": "134.8719"
        },
        "timezone": {
          "offset": "+9:30",
          "description": "Adelaide, Darwin"
        }
      },
      "email": "jennie.nichols@example.com",
      "login": {
        "uuid": "7a0eed16-9430-4d68-901f-c0d4c1c3bf00",
        "username": "yellowpeacock117",
        "password": "addison",
        "salt": "sld1yGtd",
        "md5": "ab54ac4c0be9480ae8fa5e9e2a5196a3",
        "sha1": "edcf2ce613cbdea349133c52dc2f3b83168dc51b",
        "sha256": "48df5229235ada28389b91e60a935e4f9b73eb4bdb855ef9258a1751f10bdc5d"
      },
      "dob": {
        "date": "1992-03-08T15:13:16.688Z",
        "age": 30
      },
      "registered": {
        "date": "2007-07-09T05:51:59.390Z",
        "age": 14
      },
      "phone": "(272) 790-0888",
      "cell": "(489) 330-2385",
      "id": {
        "name": "SSN",
        "value": "405-88-3636"
      },
      "picture": {
        "large": "https://randomuser.me/api/portraits/men/75.jpg",
        "medium": "https://randomuser.me/api/portraits/med/men/75.jpg",
        "thumbnail": "https://randomuser.me/api/portraits/thumb/men/75.jpg"
      },
      "nat": "US"
    }
  ],
  "info": {
    "seed": "56d27f4a53bd5441",
    "results": 1,
    "page": 1,
    "version": "1.4"
  }
}
