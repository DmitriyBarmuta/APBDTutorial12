using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tutorial12.Models;

namespace Tutorial12.Data;

public class DatabaseContext : DbContext
{
    private const string Datetime = "datetime";
    
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── CLIENT ───────────────────────────────────────────────────────────────────

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient).HasName("Client_pk");
                entity.ToTable("Client");

                entity.Property(e => e.Email)
                      .HasMaxLength(120)
                      .IsRequired();

                entity.Property(e => e.FirstName)
                      .HasMaxLength(120)
                      .IsRequired();

                entity.Property(e => e.LastName)
                      .HasMaxLength(120)
                      .IsRequired();

                entity.Property(e => e.Pesel)
                      .HasMaxLength(120)
                      .IsRequired();

                entity.Property(e => e.Telephone)
                      .HasMaxLength(120)
                      .IsRequired();
            });

            // Seed Clients
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    IdClient    = 1,
                    FirstName   = "Alice",
                    LastName    = "Novák",
                    Email       = "alice.novak@example.com",
                    Telephone   = "+48-123-456-789",
                    Pesel       = "86010112345"
                },
                new Client
                {
                    IdClient    = 2,
                    FirstName   = "Bob",
                    LastName    = "Müller",
                    Email       = "bob.mueller@example.com",
                    Telephone   = "+49-987-654-321",
                    Pesel       = "90020254321"
                },
                new Client
                {
                    IdClient    = 3,
                    FirstName   = "Carla",
                    LastName    = "Rossi",
                    Email       = "carla.rossi@example.com",
                    Telephone   = "+39-333-444-555",
                    Pesel       = "92030398765"
                }
            );


            // ── TRIP ─────────────────────────────────────────────────────────────────────

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.IdTrip).HasName("Trip_pk");
                entity.ToTable("Trip");

                entity.Property(e => e.Name)
                      .HasMaxLength(120)
                      .IsRequired();

                entity.Property(e => e.Description)
                      .HasMaxLength(220)
                      .IsRequired();

                entity.Property(e => e.DateFrom)
                      .HasColumnType(Datetime)
                      .IsRequired();

                entity.Property(e => e.DateTo)
                      .HasColumnType(Datetime)
                      .IsRequired();

                entity.Property(e => e.MaxPeople)
                      .IsRequired();
            });

            // Seed Trips
            modelBuilder.Entity<Trip>().HasData(
                new Trip
                {
                    IdTrip      = 1,
                    Name        = "Poland Spring 2025",
                    Description = "Explore Kraków & Warsaw in spring.",
                    DateFrom    = new DateTime(2025, 4, 1, 0, 0 ,0, DateTimeKind.Local),
                    DateTo      = new DateTime(2025, 4, 7, 0, 0 ,0, DateTimeKind.Local),
                    MaxPeople   = 20
                },
                new Trip
                {
                    IdTrip      = 2,
                    Name        = "Rhine River Cruise",
                    Description = "Seven‐day cruise from Basel to Amsterdam",
                    DateFrom    = new DateTime(2025, 6, 10, 0, 0 ,0, DateTimeKind.Local),
                    DateTo      = new DateTime(2025, 6, 17, 0, 0 ,0, DateTimeKind.Local),
                    MaxPeople   = 15
                },
                new Trip
                {
                    IdTrip      = 3,
                    Name        = "Italian Food Tour",
                    Description = "Taste your way through Tuscany & Milan",
                    DateFrom    = new DateTime(2025, 7, 5, 0, 0 ,0, DateTimeKind.Local),
                    DateTo      = new DateTime(2025, 7, 12, 0, 0 ,0, DateTimeKind.Local),
                    MaxPeople   = 12
                }
            );


            // ── COUNTRY ↔ TRIP (Many‐to‐Many) ───────────────────────────────────────────────

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry).HasName("Country_pk");
                entity.ToTable("Country");

                entity.Property(e => e.Name)
                      .HasMaxLength(120)
                      .IsRequired();
            });

            // Seed Countries
            modelBuilder.Entity<Country>().HasData(
                new Country { IdCountry = 1, Name = "Poland" },
                new Country { IdCountry = 2, Name = "Germany" },
                new Country { IdCountry = 3, Name = "Italy" }
            );

            modelBuilder.Entity<Country>()
                .HasMany(c => c.IdTrips)
                .WithMany(t => t.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "Country_Trip",
                    right => right
                        .HasOne<Trip>()
                        .WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Trip"),
                    left => left
                        .HasOne<Country>()
                        .WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Country"),
                    join =>
                    {
                        join.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                        join.ToTable("Country_Trip");
                        join.HasData(
                            new { IdCountry = 1, IdTrip = 1 },
                            new { IdCountry = 2, IdTrip = 2 },
                            new { IdCountry = 3, IdTrip = 3 }
                        );
                    }
                );


            // ── CLIENT_TRIP ─────────────────────────────────────────────────────────────────

            modelBuilder.Entity<ClientTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdClient, e.IdTrip })
                      .HasName("Client_Trip_pk");
                entity.ToTable("Client_Trip");

                entity.Property(e => e.RegisteredAt)
                      .HasColumnType(Datetime)
                      .IsRequired();

                entity.Property(e => e.PaymentDate)
                      .HasColumnType(Datetime)
                      .IsRequired(false);

                entity.HasOne(d => d.IdClientNavigation)
                      .WithMany(p => p.ClientTrips)
                      .HasForeignKey(d => d.IdClient)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("Table_5_Client");

                entity.HasOne(d => d.IdTripNavigation)
                      .WithMany(p => p.ClientTrips)
                      .HasForeignKey(d => d.IdTrip)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("Table_5_Trip");
            });

            // Seed Client_Trip
            modelBuilder.Entity<ClientTrip>().HasData(
                new ClientTrip
                {
                    IdClient      = 1,
                    IdTrip        = 1,
                    RegisteredAt  = new DateTime(2025, 2, 1,  10, 30,  0, DateTimeKind.Local),
                    PaymentDate   = null
                },
                new ClientTrip
                {
                    IdClient      = 2,
                    IdTrip        = 2,
                    RegisteredAt  = new DateTime(2025, 5, 1,  14, 45,  0, DateTimeKind.Local),
                    PaymentDate   = new DateTime(2025, 5,  5,  9,   0,   0, DateTimeKind.Local)
                },
                new ClientTrip
                {
                    IdClient      = 3,
                    IdTrip        = 3,
                    RegisteredAt  = new DateTime(2025, 5, 15, 11, 20,  0, DateTimeKind.Local),
                    PaymentDate   = null
                }
            );
        }
}
