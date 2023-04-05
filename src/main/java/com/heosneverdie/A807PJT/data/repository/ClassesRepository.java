package com.heosneverdie.A807PJT.data.repository;

import com.heosneverdie.A807PJT.data.entity.member.Account;
import com.heosneverdie.A807PJT.data.entity.member.Classes;
import org.springframework.data.jpa.repository.JpaRepository;


public interface ClassesRepository extends JpaRepository<Classes, Long> {
}