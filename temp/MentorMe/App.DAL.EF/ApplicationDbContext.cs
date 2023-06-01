using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.DAL.EF;

public class ApplicationDbContext: IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;
    public DbSet<AppUser> AppUsers { get; set; } = default!;
    
    public DbSet<Student> Students { get; set; } = default!;
    public DbSet<StudentPaymentMethod> StudentPaymentMethods { get; set; } = default!;
    public DbSet<StudentSubject> StudentSubjects { get; set; } = default!;
    public DbSet<Tutor> Tutors { get; set; } = default!;
    public DbSet<TutorBankingDetails> TutorBankingDetails { get; set; } = default!;
    public DbSet<TutorSubject> TutorSubjects { get; set; } = default!;
    public DbSet<TutorAvailability> TutorAvailabilities { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Subject> Subjects { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;

    public DbSet<Lesson> Lessons { get; set; } = default!;
    public DbSet<LessonPayment> LessonPayments { get; set; } = default!;
    public DbSet<LessonParticipation> LessonParticipations { get; set; } = default!;
    public DbSet<Cancellation> Cancellations { get; set; } = default!;
    public DbSet<Payment> Payments { get; set; } = default!;
    
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<Dialog> Dialogs { get; set; } = default!;
    public DbSet<DialogParticipant> DialogParticipants { get; set; } = default!;
    public DbSet<Notification> Notifications { get; set; } = default!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Remove Cascade delete on all the entities
        foreach (var relationship in modelBuilder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        if (!optionsBuilder.IsConfigured)
        {
            // optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("SQLiteConnection"));
        }
    }
    private readonly IConfiguration _configuration;
    
}
