package com.heosneverdie.A807PJT.data.repository;

import com.heosneverdie.A807PJT.data.entity.member.Account;
import com.heosneverdie.A807PJT.data.entity.member.Member;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;


public interface AccountRepository extends JpaRepository<Account, Long> {
}