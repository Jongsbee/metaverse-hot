package com.heosneverdie.A807PJT.data.repository;

import com.heosneverdie.A807PJT.data.entity.member.Member;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;


public interface MemberRepository extends JpaRepository<Member, Long> {
    Optional<Member> findByNickname(String nickname);
}