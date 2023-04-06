package com.heosneverdie.A807PJT.data.repository;

import com.heosneverdie.A807PJT.data.entity.member.Coupon;
import com.heosneverdie.A807PJT.data.entity.member.Member;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;


public interface CouponRepository extends JpaRepository<Coupon, Long> {
    Optional<Coupon> findByMemberId(Long memberId);
}