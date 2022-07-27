// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using UnigisTest;
using UnigisTest.Models;
using Newtonsoft.Json;

Console.WriteLine("Iniciando!!!");
UnigisLog.GetInstance().Log("Inicio", "Main", $"Iniciando proceso UnigisTest {DateTime.Now}");
UnigisTest().GetAwaiter().GetResult();


static async Task UnigisTest()
{
    try
    {
        bool response = false;
        string message = "No fue posible recuperar datos del servicio REST, consulte el log para mas detalles.";

        response = await GetRazas();

        if (!response)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            UnigisLog.GetInstance().Log("Error", "UnigisTest", "No fue posible obtener datos de la funcion GetRazas");
            return;
        }

        response = await GetImages();

        if (!response)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            UnigisLog.GetInstance().Log("Error", "UnigisTest", "No fue posible obtener datos de la funcion GetImages");
            return;
        }

        UnigisLog.GetInstance().Log("Fin", "Main", $"Finalizando proceso UnigisTest {DateTime.Now}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Ejecución realizada con exito.");

    }
    catch (Exception ex)
    {
        UnigisLog.GetInstance().Log("Error", "UnigisTest", ex.Message);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
    }
}

static async Task<bool> GetRazas()
{
    Console.WriteLine("Obteniedo Razas");

    bool response = false;

    ResponseAPI responseAPI = await UnigisService.GetInstance().Get("list");

    if (responseAPI != null)
    {
        if (responseAPI.Status!.Equals("success"))
        {
            foreach (string nombreRaza in responseAPI.Message!)
            {
                using (var unigisContext = new UnigisContext())
                {
                    unigisContext.Database.Log = s => UnigisLog.GetInstance().Log("OK", "GetRazas", s);

                    if (!unigisContext.Razas.Any(x => x.NombreRaza!.Equals(nombreRaza)))
                    {
                        Raza raza = new Raza()
                        {
                            NombreRaza = nombreRaza
                        };

                        unigisContext.Razas.Add(raza);
                        unigisContext.SaveChanges();
                    }
                    else
                    {
                        UnigisLog.GetInstance().Log("Error", "GetRazas", $"La raza {nombreRaza} ya se encuentra registrada en la tabla Razas");
                    }
                }
            }

            response = true;
        }
        else
        {
            UnigisLog.GetInstance().Log("Error", "GetRazas", "El valor status es diferente a success");
        }
    }
    else
    {
        UnigisLog.GetInstance().Log("Error", "GetRazas", "El valor devuelto debe ser diferente a Null");
    }

    return response;
}

static async Task<bool> GetImages()
{
    Console.WriteLine("Obteniendo imagenes");

    bool response = false;

    ResponseAPI responseAPI = await UnigisService.GetInstance().Get("images");

    if (responseAPI != null)
    {
        if (responseAPI.Status!.Equals("success"))
        {
            using (var unigisContext = new UnigisContext())
            {
                unigisContext.Database.Log = s => UnigisLog.GetInstance().Log("OK", "GetImages", s);

                List<Raza> razas = unigisContext.Razas.ToList();

                foreach (Raza raza in razas)
                {
                    List<FotoRaza> fotosRaza = new List<FotoRaza>();

                    List<string> urlsRaza = responseAPI.Message!.Where(x => x.Contains(raza.NombreRaza!)).ToList();

                    foreach (string url in urlsRaza)
                    {

                        if (!fotosRaza.Any(x => x.RutaImagen!.Equals(url)))
                        {
                            if(!unigisContext.FotosRaza.Any(x => x.RutaImagen!.Equals(url)))
                            {
                                FotoRaza fotoRaza = new FotoRaza()
                                {
                                    RutaImagen = url,
                                    Raza = raza
                                };

                                fotosRaza.Add(fotoRaza);
                            }
                            else
                            {
                                UnigisLog.GetInstance().Log("Error", "GetImages", $"La imagen {url} ya se encuentra registrada para la raza {raza.NombreRaza} en la tabla FotoRazas");
                            }
                        }
                        else
                        {
                            UnigisLog.GetInstance().Log("Error", "GetImages", "Ruta de imagen duplicada");
                        }
                    }

                    unigisContext.FotosRaza.AddRange(fotosRaza);
                    unigisContext.SaveChanges();

                }

                response = true;
            }

        }
        else
        {
            UnigisLog.GetInstance().Log("Error", "GetImages", "El valor status es diferente a success");
        }
    }
    else
    {
        UnigisLog.GetInstance().Log("Error", "GetImages", "El valor devuelto debe ser diferente a Null");
    }

    return response;
}
