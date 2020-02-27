
                    namespace Components.RoslynJit
                    {
                        using System;
                        using System.Collections.Generic;

                        public static class GeneratedContainer
                        {
                            
                                public readonly struct CityInfo
                                {
                                    public readonly string Name;
                                    public readonly string State;
                                    public const string Country = "JE";
                                    public readonly double Latitude;
                                    public readonly double Longitude;

                                    public CityInfo(
                                        string name,
                                        in string state,
                                        double latitude,
                                        double longitude)
                                    {
                                        Name = name;
                                        State = state;
                                        Latitude = latitude;
                                        Longitude = longitude;
                                    }
                                }
                            
                                public static CityInfo GetClosestCity(double lat, double lng)
                                {
                                    double tmp;
                            
                                        var cur = (Math.Abs(49.18804 - lat) + Math.Abs(-2.10491 - lng));
                                        var closest = 0;
                                    
                                        if ((tmp = (Math.Abs(49.16823 - lat) + Math.Abs(-2.06178 - lng))) < cur)
                                        {
                                            cur = tmp;
                                            closest = 1;
                                        }
                                    
                                    switch (closest)
                                    {
                            case 0: return new CityInfo("Saint Helier", "3237864", 49.18804, -2.10491);
default: return new CityInfo("Le Hocq", "3237072", 49.16823, -2.06178);

                                    }                                        
                                }
                            
                        }
                    }
                