using System;
using System.Collections.Generic;
using Diploma.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Diploma.DataAccess
{
    public partial class DiplomDbContext : DbContext
    {
        public DiplomDbContext(DbContextOptions<DiplomDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<ModalType> ModalTypes { get; set; } = null!;
        public virtual DbSet<Personality> Personalities { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;
        public virtual DbSet<TestQuestion> TestQuestions { get; set; } = null!;
        public virtual DbSet<TestResult> TestResults { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.HasIndex(e => e.AnswerId, "Answer_answer_id_uindex")
                    .IsUnique();

                entity.Property(e => e.AnswerId)
                    .ValueGeneratedNever()
                    .HasColumnName("answer_id");

                entity.Property(e => e.AnswerResult).HasColumnName("answer_result");

                entity.Property(e => e.AnswerText).HasColumnName("answer_text");

                entity.Property(e => e.AnswerTextResult)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("answer_text_result")
                    .IsFixedLength();
            });

            modelBuilder.Entity<ModalType>(entity =>
            {
                entity.ToTable("Modal_Type");

                entity.HasIndex(e => e.ModalTypeId, "Modal_Type_modal_type_id_uindex")
                    .IsUnique();

                entity.Property(e => e.ModalTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("modal_type_id");

                entity.Property(e => e.ModalTypeName).HasColumnName("modal_type_name");
            });

            modelBuilder.Entity<Personality>(entity =>
            {
                entity.ToTable("Personality");

                entity.HasIndex(e => e.PersonalityId, "Personality_personality_id_uindex")
                    .IsUnique();

                entity.Property(e => e.PersonalityId)
                    .ValueGeneratedNever()
                    .HasColumnName("personality_id");

                entity.Property(e => e.PersonalityDescription).HasColumnName("personality_description");

                entity.Property(e => e.PersonalityLink).HasColumnName("personality_link");

                entity.Property(e => e.PersonalityTitle).HasColumnName("personality_title");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.HasIndex(e => e.QuestionId, "Question_question_id_uindex")
                    .IsUnique();

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("question_id");

                entity.Property(e => e.QuestionNumber).HasColumnName("question_number");

                entity.Property(e => e.QuestionText).HasColumnName("question_text");
            });

            modelBuilder.Entity<QuestionAnswer>(entity =>
            {
                entity.ToTable("Question_Answer");

                entity.HasIndex(e => e.AnswerId, "IX_Question_Answer_answer_id");

                entity.HasIndex(e => e.QuestionId, "IX_Question_Answer_question_id");

                entity.HasIndex(e => e.Id, "Question_Answer_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AnswerId).HasColumnName("answer_id");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.QuestionAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("Question_Answer_Answer_answer_id_fk");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("Question_Answer_Question_question_id_fk");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("Test");

                entity.HasIndex(e => e.TestId, "Test_test_id_uindex")
                    .IsUnique();

                entity.Property(e => e.TestId)
                    .ValueGeneratedNever()
                    .HasColumnName("test_id");

                entity.Property(e => e.TestType).HasColumnName("test_type");
            });

            modelBuilder.Entity<TestQuestion>(entity =>
            {
                entity.ToTable("Test_Question");

                entity.HasIndex(e => e.QuestionId, "IX_Test_Question_question_id");

                entity.HasIndex(e => e.TestId, "IX_Test_Question_test_id");

                entity.HasIndex(e => e.Id, "Test_Question_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TestQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("Test_Question_Question_question_id_fk");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestQuestions)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("Test_Question_Test_test_id_fk");
            });

            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.ToTable("Test_Result");

                entity.Property(e => e.TestResultId)
                    .ValueGeneratedNever()
                    .HasColumnName("test_result_id");

                entity.Property(e => e.ModalResult).HasColumnName("modal_result");

                entity.Property(e => e.ModalTimeResult).HasColumnName("modal_time_result");

                entity.Property(e => e.TestNumber).HasColumnName("test_number");

                entity.Property(e => e.TestResultt).HasColumnName("test_resultt");

                entity.Property(e => e.TestTimeResult).HasColumnName("test_time_result");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Test_Result_User_null_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.ModalTypeId, "IX_User_modal_type_id");

                entity.HasIndex(e => e.PersonalityId, "IX_User_personality_id");

                entity.HasIndex(e => e.TestId, "IX_User_test_id");

                entity.HasIndex(e => e.UserId, "User_user_id_uindex")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.ClarifyingQuestionOne).HasColumnName("clarifying_question_one");

                entity.Property(e => e.ClarifyingQuestionThree).HasColumnName("clarifying_question_three");

                entity.Property(e => e.ClarifyingQuestionTwo).HasColumnName("clarifying_question_two");

                entity.Property(e => e.ModalTypeId).HasColumnName("modal_type_id");

                entity.Property(e => e.PersonalityId).HasColumnName("personality_id");

                entity.Property(e => e.TestId).HasColumnName("test_id");

                entity.Property(e => e.TestTimeResult).HasColumnName("test_time_result");

                entity.Property(e => e.UserCreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("user_create_date");

                entity.HasOne(d => d.ModalType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ModalTypeId)
                    .HasConstraintName("User_Modal_Type_modal_type_id_fk");

                entity.HasOne(d => d.Personality)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PersonalityId)
                    .HasConstraintName("User_Personality_personality_id_fk");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("User_Test_test_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
