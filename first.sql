CREATE TABLE IF NOT EXISTS public."__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;
CREATE TABLE public.courses (
    id uuid NOT NULL,
    code character varying(7) NOT NULL,
    title character varying(50) NOT NULL,
    unit integer NOT NULL,
    description text NOT NULL,
    CONSTRAINT pk_courses PRIMARY KEY (id)
);

CREATE TABLE public.departmental_courses (
    id uuid NOT NULL,
    department_id uuid NOT NULL,
    course_id uuid NOT NULL,
    level integer NOT NULL,
    semester integer NOT NULL,
    status character varying(10) NOT NULL,
    CONSTRAINT pk_departmental_courses PRIMARY KEY (id)
);

CREATE TABLE public.schools (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    abbr character varying(5) NOT NULL,
    CONSTRAINT pk_schools PRIMARY KEY (id)
);

CREATE TABLE public.departments (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    abbr character varying(4) NOT NULL,
    school_id uuid NOT NULL,
    CONSTRAINT pk_departments PRIMARY KEY (id),
    CONSTRAINT fk_departments_schools_school_id FOREIGN KEY (school_id) REFERENCES public.schools (id) ON DELETE CASCADE
);

CREATE TABLE public.students (
    id uuid NOT NULL,
    reg_number character varying(11) NOT NULL,
    first_name character varying(20) NOT NULL,
    last_name character varying(20) NOT NULL,
    date_of_birth date NOT NULL,
    email character varying(70) NOT NULL,
    phone_number character varying(11) NOT NULL,
    entry_session character varying(9) NOT NULL,
    current_level integer NOT NULL,
    department_id uuid NOT NULL,
    CONSTRAINT pk_students PRIMARY KEY (id),
    CONSTRAINT fk_students_departments_department_id FOREIGN KEY (department_id) REFERENCES public.departments (id) ON DELETE CASCADE
);

CREATE TABLE public.course_registrations (
    id uuid NOT NULL,
    student_id uuid NOT NULL,
    course_id uuid NOT NULL,
    session character varying(9) NOT NULL,
    semester integer NOT NULL,
    registration_date timestamp with time zone NOT NULL,
    CONSTRAINT pk_course_registrations PRIMARY KEY (id),
    CONSTRAINT fk_course_registrations_courses_course_id FOREIGN KEY (course_id) REFERENCES public.courses (id) ON DELETE CASCADE,
    CONSTRAINT fk_course_registrations_students_student_id FOREIGN KEY (student_id) REFERENCES public.students (id) ON DELETE CASCADE
);

CREATE INDEX ix_course_registrations_course_id ON public.course_registrations (course_id);

CREATE UNIQUE INDEX ix_course_registrations_student_id_course_id_session ON public.course_registrations (student_id, course_id, session);

CREATE UNIQUE INDEX ix_courses_code ON public.courses (code);

CREATE UNIQUE INDEX ix_departmental_courses_department_id_course_id ON public.departmental_courses (department_id, course_id);

CREATE INDEX ix_departments_school_id ON public.departments (school_id);

CREATE INDEX ix_students_department_id ON public.students (department_id);

CREATE UNIQUE INDEX ix_students_reg_number ON public.students (reg_number);

INSERT INTO public."__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20261029214404_FirstMigration', '10.0.13');

COMMIT;

